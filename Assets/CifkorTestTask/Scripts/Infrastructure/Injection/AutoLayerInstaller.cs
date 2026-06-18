using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Infrastructure.Injection
{
    public abstract class AutoLayerInstaller : MonoInstaller
    {
        private static string Namespace { get; set; }
        private static string RootNamespace { get; set; }

        //TODO: Некритично для прототипов, но можно переделать в код-ген,
        //чтобы не искать реализации через рефлексию на старте игры.
        public override void InstallBindings()
        {
            Debug.Log($"--- Running: {GetType().Name}");

            Namespace = GetType().Namespace;
            RootNamespace = GetBeforeLastDot(Namespace);

            var installers = FindAutoInstallers();

            foreach (var installer in installers)
            {
                Debug.Log($"Installing: {installer.GetType().Name}");
                installer.InstallBindings(Container);
            }
        }

        private IAutoInstaller[] FindAutoInstallers()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(GetSafeTypes)
                .Where(t => typeof(IAutoInstaller).IsAssignableFrom(t)
                            && t.IsClass
                            && !t.IsAbstract &&
                            IsInNamespaceTree(t.Namespace))
                .Select(t => (IAutoInstaller)Activator.CreateInstance(t))
                .ToArray();
        }

        private static bool IsInNamespaceTree(string typeNamespace)
        {
            if (string.IsNullOrEmpty(typeNamespace) || string.IsNullOrEmpty(Namespace))
                return false;

            return typeNamespace.StartsWith(Namespace) || typeNamespace.StartsWith(RootNamespace);
        }

        private static string GetBeforeLastDot(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var lastDotIndex = input.LastIndexOf('.');

            return lastDotIndex == -1
                ? input
                : input[..lastDotIndex];
        }

        private static Type[] GetSafeTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using CifkorTestTask.Infrastructure.Configs;
using CifkorTestTask.Infrastructure.Injection;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Application.Configs.Installer
{
    public class ConfigsInstaller: IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            var configTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(GetSafeTypes)
                .Where(IsConfigSOType);

            foreach (var configSoType in configTypes)
            {
                var asset = Resources.LoadAll(string.Empty, configSoType)
                    .FirstOrDefault();

                if (asset == null)
                {
                    Debug.LogWarning($"Config asset not found for {configSoType.Name}");
                    continue;
                }

                var configProperty = configSoType.GetProperty("Config");
                var configValue = configProperty?.GetValue(asset);

                if (configValue == null)
                    continue;

                var configType = configProperty.PropertyType;

                container.Bind(configType)
                    .FromInstance(configValue)
                    .AsSingle();

                Debug.Log($"Bound config: {configType.Name}");
            }
        }

        private static bool IsConfigSOType(Type type)
        {
            if (!typeof(ScriptableObject).IsAssignableFrom(type) ||
                type.IsAbstract)
            {
                return false;
            }

            var current = type;

            while (current != null)
            {
                if (current.IsGenericType &&
                    current.GetGenericTypeDefinition() == typeof(BaseScriptableObjectConfig<>))
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
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

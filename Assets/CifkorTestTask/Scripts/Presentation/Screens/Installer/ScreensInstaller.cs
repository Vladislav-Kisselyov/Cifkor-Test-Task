using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Screens.Installer
{
    public class ScreensInstaller : MonoInstaller
    {
        [SerializeField]
        private List<ViewBase> _views;

        public override void InstallBindings()
        {
            Debug.Log($"--- Running {nameof(ScreensInstaller)}");
            foreach (var view in _views)
            {
                var type = view.GetType();
                var interfaces = type.GetInterfaces();

                foreach (var iface in interfaces)
                {
                    Debug.Log($"Binding {iface.Name} to {type.Name}");

                    Container.Bind(iface)
                        .FromInstance(view);
                }
            }
        }
    }
}

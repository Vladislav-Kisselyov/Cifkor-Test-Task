using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Infrastructure.Injection.Internal;
using Zenject;

namespace CifkorTestTask.Presentation.EntryPoint.Installer
{
    internal class EntryPointInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesTo<ZenjectFactory>()
                .AsSingle()
                .NonLazy();
            container.BindInterfacesTo<EntryPoint>()
                .AsSingle()
                .NonLazy();
        }
    }
}

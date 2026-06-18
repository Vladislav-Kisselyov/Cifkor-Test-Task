using CifkorTestTask.Application.Pets.Internal;
using CifkorTestTask.Infrastructure.Injection;
using Zenject;

namespace CifkorTestTask.Application.Pets.Installer
{
    internal class BreedApplicationInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesTo<BreedController>().AsSingle();
        }
    }
}

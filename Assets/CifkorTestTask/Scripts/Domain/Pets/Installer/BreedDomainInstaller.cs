using CifkorTestTask.Infrastructure.Injection;
using Zenject;

namespace CifkorTestTask.Domain.Pets.Installer
{
    internal class BreedDomainInstaller: IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.Bind<Breed>().AsSingle();
        }
    }
}

using CifkorTestTask.Infrastructure.Injection;
using Zenject;

namespace CifkorTestTask.Domain.Climate.Installer
{
    internal class ClimateDomainInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.Bind<Weather>().AsSingle();
        }
    }
}

using CifkorTestTask.Application.Climate.Internal;
using CifkorTestTask.Infrastructure.Injection;
using Zenject;

namespace CifkorTestTask.Application.Climate.Installer
{
    internal class ClimateApplicationInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesTo<WeatherController>().AsSingle();
        }
    }
}

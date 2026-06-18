using CifkorTestTask.Application.Cookie.Internal;
using CifkorTestTask.Infrastructure.Injection;
using Zenject;

namespace CifkorTestTask.Application.Cookie.Installer
{
    internal class ClickerApplicationInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesTo<ClickerController>().AsSingle();
        }
    }
}

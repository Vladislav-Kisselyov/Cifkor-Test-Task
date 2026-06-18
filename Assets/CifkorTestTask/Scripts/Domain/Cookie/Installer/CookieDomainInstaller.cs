using CifkorTestTask.Infrastructure.Injection;
using Zenject;

namespace CifkorTestTask.Domain.Cookie.Installer
{
    internal class CookieDomainInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.Bind<Clicker>().AsSingle();
        }
    }
}

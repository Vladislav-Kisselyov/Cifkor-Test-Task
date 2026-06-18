using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Infrastructure.Networking.Internal;
using Zenject;

namespace CifkorTestTask.Infrastructure.Networking.Installer
{
    internal class NetworkingInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesTo<UnityHttpClient>().AsSingle().NonLazy();
            container.BindInterfacesTo<RequestQueueService>().AsSingle().NonLazy();
        }
    }
}

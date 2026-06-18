using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Infrastructure.Pooling.Internal;
using Zenject;

namespace CifkorTestTask.Infrastructure.Pooling.Installer
{
    internal class PoolingInstaller: IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesTo<PoolableFactory>().AsSingle().NonLazy();
        }
    }
}

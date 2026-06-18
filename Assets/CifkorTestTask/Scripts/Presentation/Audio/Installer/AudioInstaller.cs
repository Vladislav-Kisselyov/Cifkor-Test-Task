using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Presentation.Audio.Internal;
using Zenject;

namespace CifkorTestTask.Presentation.Audio.Installer
{
    internal class AudioInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesTo<AudioController>()
                .AsSingle()
                .NonLazy();
        }
    }
}

using Zenject;

namespace CifkorTestTask.Infrastructure.Injection
{
    public interface IAutoInstaller
    {
        void InstallBindings(DiContainer container);
    }
}

using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Presentation.Navigation.Presenter;
using Zenject;

namespace CifkorTestTask.Presentation.EntryPoint
{
    public class EntryPoint : IInitializable
    {
        private readonly IInjectionFactory _factory;

        public EntryPoint(IInjectionFactory factory)
        {
            _factory = factory;
        }

        void IInitializable.Initialize()
        {
            _factory.Create<NavigationPresenter>();
        }
    }
}

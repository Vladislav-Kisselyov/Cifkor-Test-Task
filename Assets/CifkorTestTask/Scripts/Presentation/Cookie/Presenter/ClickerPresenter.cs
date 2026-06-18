using System;
using CifkorTestTask.Application.Cookie;
using CifkorTestTask.Presentation.Cookie.View;
using Zenject;

namespace CifkorTestTask.Presentation.Cookie.Presenter
{
    public class ClickerPresenter : IInitializable, IDisposable
    {
        private readonly IClickerController _controller;
        private readonly IClickerView _view;

        public ClickerPresenter(
            IClickerController controller,
            IClickerView view)
        {
            _controller = controller;
            _view = view;
        }

        void IInitializable.Initialize()
        {
            _controller.OnCurrencyChanged += HandleCurrencyChanged;
            _controller.OnEnergyChanged += HandleEnergyChanged;
            _controller.OnTapExecuted += HandleTapExecuted;
            _controller.OnAutoCollectExecuted += HandleAutoCollectExecuted;

            _view.OnButtonClick += HandleViewButtonClick;
            _view.SetCurrency(_controller.Currency);
            _view.SetEnergy(_controller.Energy);
            _view.Show();
        }

        void IDisposable.Dispose()
        {
            _controller.OnCurrencyChanged -= HandleCurrencyChanged;
            _controller.OnEnergyChanged -= HandleEnergyChanged;
            _controller.OnTapExecuted -= HandleTapExecuted;
            _controller.OnAutoCollectExecuted -= HandleAutoCollectExecuted;

            _view.OnButtonClick -= HandleViewButtonClick;
            _view.Hide();
        }

        private void HandleViewButtonClick()
        {
            _controller.TryTap();
        }

        private void HandleCurrencyChanged(int newCurrencyValue)
        {
            _view.SetCurrency(newCurrencyValue);
        }

        private void HandleEnergyChanged(int newEnergyValue)
        {
            _view.SetEnergy(newEnergyValue);
        }

        private void HandleTapExecuted()
        {

        }

        private void HandleAutoCollectExecuted()
        {

        }
    }
}

using System;
using CifkorTestTask.Application.Cookie;
using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Presentation.Audio;
using CifkorTestTask.Presentation.Cookie.View;
using Zenject;

namespace CifkorTestTask.Presentation.Cookie.Presenter
{
    public class ClickerPresenter : IInitializable, IDisposable
    {
        private readonly IInjectionFactory _injectionFactory;
        private readonly IAudioController _audioController;
        private readonly IClickerController _clickerController;
        private readonly IClickerView _view;

        private ParticlesPanelPresenter _particlesPanelPresenter;

        public ClickerPresenter(
            IInjectionFactory injectionFactory,
            IAudioController audioController,
            IClickerController clickerController,
            IClickerView view)
        {
            _injectionFactory = injectionFactory;
            _audioController = audioController;
            _clickerController = clickerController;
            _view = view;
        }

        void IInitializable.Initialize()
        {
            _particlesPanelPresenter = _injectionFactory.Create<ParticlesPanelPresenter>();

            _clickerController.OnCurrencyChanged += HandleCurrencyChanged;
            _clickerController.OnEnergyChanged += HandleEnergyChanged;
            _clickerController.OnTapExecuted += HandleTapExecuted;
            _clickerController.OnAutoCollectExecuted += HandleAutoCollectExecuted;

            _view.OnButtonClick += HandleViewButtonClick;
            _view.SetCurrency(_clickerController.Currency);
            _view.SetEnergy(_clickerController.Energy);
            _view.Show();
        }

        void IDisposable.Dispose()
        {
            _particlesPanelPresenter.Dispose();

            _clickerController.OnCurrencyChanged -= HandleCurrencyChanged;
            _clickerController.OnEnergyChanged -= HandleEnergyChanged;
            _clickerController.OnTapExecuted -= HandleTapExecuted;
            _clickerController.OnAutoCollectExecuted -= HandleAutoCollectExecuted;

            _view.OnButtonClick -= HandleViewButtonClick;
            _view.Hide();
        }

        private void HandleViewButtonClick()
        {
            _clickerController.TryTap();
        }

        private void HandleCurrencyChanged(int newCurrencyValue, int deltaValue)
        {
            _particlesPanelPresenter.ShowParticle(
                deltaValue,
                _view.CurrencySprite,
                _view.ButtonTransform,
                _view.CurrencyTransform,
                () =>
                {
                    _view.SetCurrency(newCurrencyValue);
                    _audioController.Play(AudioClipKeys.CurrencyUpdate);
                });
        }

        private void HandleEnergyChanged(int newEnergyValue)
        {
            _view.SetEnergy(newEnergyValue);
        }

        private void HandleTapExecuted()
        {
            _view.PlayButtonBounceAnimation();
            _audioController.Play(AudioClipKeys.SuccessManualTap);
        }

        private void HandleAutoCollectExecuted()
        {
            _audioController.Play(AudioClipKeys.SuccessAutoCollect);
        }
    }
}

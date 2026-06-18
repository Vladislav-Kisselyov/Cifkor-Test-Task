using System;
using System.Threading;
using CifkorTestTask.Domain.Cookie;
using CifkorTestTask.Domain.Cookie.Config;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CifkorTestTask.Application.Cookie.Internal
{
    internal class ClickerController : IClickerController, IInitializable, IDisposable
    {
        private readonly ClickerConfig _clickerConfig;
        private readonly Clicker _clickerDomain;

        private CancellationTokenSource _lifetimeCts;

        public int Currency => _clickerDomain.RuntimeData.Currency;
        public int Energy => _clickerDomain.RuntimeData.Energy;

        public event Action<int> OnCurrencyChanged;
        public event Action<int> OnEnergyChanged;
        public event Action OnTapExecuted;
        public event Action OnAutoCollectExecuted;

        public ClickerController(
            ClickerConfig clickerConfig,
            Clicker clickerDomain)
        {
            _clickerConfig = clickerConfig;
            _clickerDomain = clickerDomain;
        }

        void IInitializable.Initialize()
        {
            _lifetimeCts = new CancellationTokenSource();

            _clickerDomain.OnCurrencyChanged += HandleCurrencyChanged;
            _clickerDomain.OnEnergyChanged += HandleEnergyChanged;
            _clickerDomain.OnTapExecuted += HandleTapExecuted;
            _clickerDomain.OnAutoCollectExecuted += HandleAutoCollectExecuted;

            AutoCollectLoopAsync(_lifetimeCts.Token).Forget();
            EnergyRegenLoopAsync(_lifetimeCts.Token).Forget();
        }

        void IDisposable.Dispose()
        {
            _clickerDomain.OnCurrencyChanged -= HandleCurrencyChanged;
            _clickerDomain.OnEnergyChanged -= HandleEnergyChanged;
            _clickerDomain.OnTapExecuted -= HandleTapExecuted;
            _clickerDomain.OnAutoCollectExecuted -= HandleAutoCollectExecuted;

            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
        }

        public void TryTap()
        {
            _clickerDomain.TryTap();
        }

        private async UniTaskVoid AutoCollectLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(
                    _clickerConfig.AutoCollectIntervalSeconds,
                    cancellationToken: ct);

                if (ct.IsCancellationRequested)
                    break;

                _clickerDomain.TryAutoCollect();
            }
        }

        private async UniTaskVoid EnergyRegenLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(
                    _clickerConfig.EnergyRegenIntervalSeconds,
                    cancellationToken: ct);

                if (ct.IsCancellationRequested)
                    break;

                _clickerDomain.RegenEnergy();
            }
        }

        private void HandleCurrencyChanged(int newCurrencyValue)
        {
            OnCurrencyChanged?.Invoke(newCurrencyValue);
        }

        private void HandleEnergyChanged(int newEnergyValue)
        {
            OnEnergyChanged?.Invoke(newEnergyValue);
        }

        private void HandleTapExecuted()
        {
            OnTapExecuted?.Invoke();
        }

        private void HandleAutoCollectExecuted()
        {
            OnAutoCollectExecuted?.Invoke();
        }
    }
}

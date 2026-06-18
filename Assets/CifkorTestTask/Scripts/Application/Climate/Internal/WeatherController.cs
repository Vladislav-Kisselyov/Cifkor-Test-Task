using System;
using System.Threading;
using CifkorTestTask.Application.Climate.Networking;
using CifkorTestTask.Domain.Climate;
using CifkorTestTask.Domain.Climate.Data;
using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Infrastructure.Networking;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Application.Climate.Internal
{
   internal class WeatherController : IWeatherController, IInitializable, IDisposable
    {
        private const int PollIntervalMs = 5000;

        private readonly Weather _domain;
        private readonly IRequestQueueService _queue;
        private readonly IInjectionFactory _factory;

        private CancellationTokenSource _lifetimeCts;
        private CancellationTokenSource _pollCts;

        public event Action OnWeatherUpdateEnqueue;
        public event Action<WeatherData> OnWeatherUpdate;
        public event Action<string> OnWeatherError;

        public WeatherController(
            Weather domain,
            IRequestQueueService queue,
            IInjectionFactory factory)
        {
            _domain = domain;
            _queue = queue;
            _factory = factory;
        }

        void IInitializable.Initialize()
        {
            _lifetimeCts = new CancellationTokenSource();

            _domain.OnWeatherUpdate += HandleWeatherDomainUpdate;
            _domain.OnWeatherError += HandleWeatherDomainError;
        }

        void IDisposable.Dispose()
        {
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            _pollCts?.Cancel();
            _pollCts?.Dispose();

            _domain.OnWeatherUpdate -= HandleWeatherDomainUpdate;
            _domain.OnWeatherError -= HandleWeatherDomainError;
        }

        public void StartPolling()
        {
            _pollCts?.Cancel();
            _pollCts?.Dispose();
            _pollCts = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeCts.Token);
            PollLoopAsync(_pollCts.Token).Forget();
        }

        public void StopPolling()
        {
            _queue.CancelWhere(r => r is BaseTaggedRequest<WeatherData> { Tag: WeatherRequest.WeatherTag });
            _pollCts?.Cancel();
            _pollCts?.Dispose();
            _pollCts = null;
        }

        private async UniTaskVoid PollLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                EnqueueWeatherFetch(ct).Forget();
                await UniTask.Delay(PollIntervalMs, cancellationToken: ct);
            }
        }

        private async UniTaskVoid EnqueueWeatherFetch(CancellationToken ct)
        {
            var request = _factory.Create<WeatherRequest>();
            try
            {
                OnWeatherUpdateEnqueue?.Invoke();
                var data = await _queue.Enqueue(request);

                if (ct.IsCancellationRequested)
                    return;

                _domain.SetWeatherData(data);
            }
            catch (OperationCanceledException)
            {
                /* закрылась вкладка или игра */
            }
            catch (Exception ex)
            {
                _domain.SetError(ex.Message);
            }
        }

        private void HandleWeatherDomainUpdate(WeatherData data)
        {
            Debug.Log($"[WeatherController] Handling weather domain update");
            OnWeatherUpdate?.Invoke(data);
        }

        private void HandleWeatherDomainError(string error)
        {
            Debug.LogError($"[WeatherController] Handling weather domain error: {error}");
            OnWeatherError?.Invoke(error);
        }
    }
}

using System;
using System.Threading;
using CifkorTestTask.Application.Climate;
using CifkorTestTask.Domain.Climate.Data;
using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Infrastructure.Networking;
using CifkorTestTask.Presentation.Climate.Networking;
using CifkorTestTask.Presentation.Climate.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Climate.Presenter
{
    public class WeatherPresenter : IInitializable, IDisposable
    {
        private readonly IWeatherController _weatherController;
        private readonly IWeatherView _view;
        private readonly IRequestQueueService _queue;
        private readonly IInjectionFactory _factory;

        private CancellationTokenSource _lifetimeCts;
        private CancellationTokenSource _iconLoadCts;

        private string _latestIconUrl = string.Empty;

        public WeatherPresenter(
            IWeatherController weatherController,
            IWeatherView view,
            IRequestQueueService queue,
            IInjectionFactory factory)
        {
            _weatherController = weatherController;
            _view = view;
            _queue = queue;
            _factory = factory;
        }

        void IInitializable.Initialize()
        {
            _lifetimeCts = new CancellationTokenSource();

            _weatherController.OnWeatherUpdateEnqueue += HandleWeatherUpdateEnqueue;
            _weatherController.OnWeatherUpdate += HandleWeatherUpdate;
            _weatherController.OnWeatherError += HandleWeatherError;

            _weatherController.StartPolling();

            _view.Show();
            _view.SetText("Получаем данные");
            _view.SetIconDefault();
        }

        void IDisposable.Dispose()
        {
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            CancelIconFetch();

            _weatherController.OnWeatherUpdateEnqueue -= HandleWeatherUpdateEnqueue;
            _weatherController.OnWeatherUpdate -= HandleWeatherUpdate;
            _weatherController.OnWeatherError -= HandleWeatherError;

            _weatherController.StopPolling();

            _view.Hide();
            _view.Dispose();
        }

        private void EnqueueWeatherIconFetch(string label)
        {
            CancelIconFetch();

            _iconLoadCts = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeCts.Token);
            EnqueueWeatherIconFetchAsync(label, _iconLoadCts.Token).Forget();

            async UniTaskVoid EnqueueWeatherIconFetchAsync(string labelToSet, CancellationToken ct)
            {
                var request = _factory.Create<WeatherIconRequest, string>(_latestIconUrl);
                try
                {
                    var sprite = await _queue.Enqueue(request);

                    if (ct.IsCancellationRequested)
                        return;

                    _view.SetLoading(false);
                    _view.SetText(labelToSet);
                    _view.SetIcon(sprite);
                }
                catch (OperationCanceledException)
                {
                    /* закрылась вкладка или игра */
                }
                catch (Exception)
                {
                    _view.SetLoading(false);
                    _view.ShowError("Не удалось загрузить иконку погоды");
                }
            }
        }

        private void CancelIconFetch()
        {
            _queue.CancelWhere(r => r is BaseTaggedRequest<Sprite> { Tag: WeatherIconRequest.RequestTag });

            _iconLoadCts?.Cancel();
            _iconLoadCts?.Dispose();
            _iconLoadCts = null;
        }

        private void HandleWeatherUpdateEnqueue()
        {
            _view.SetLoading(true);
        }

        private void HandleWeatherUpdate(WeatherData data)
        {
            var label = $"Сегодня - {data.TemperatureF}°F";
            if (_latestIconUrl == data.IconUrl)
            {
                _view.SetLoading(false);
                _view.SetText(label);
                return;
            }

            _latestIconUrl = data.IconUrl;
            EnqueueWeatherIconFetch(label);
        }

        private void HandleWeatherError(string error)
        {
            _view.ShowError("Не удалось загрузить погоду");
        }
    }
}

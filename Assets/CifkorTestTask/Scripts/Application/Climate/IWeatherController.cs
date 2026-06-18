using System;
using CifkorTestTask.Domain.Climate.Data;

namespace CifkorTestTask.Application.Climate
{
    public interface IWeatherController
    {
        public event Action OnWeatherUpdateEnqueue;
        public event Action<WeatherData> OnWeatherUpdate;
        public event Action<string> OnWeatherError;

        public void StartPolling();
        public void StopPolling();
    }
}

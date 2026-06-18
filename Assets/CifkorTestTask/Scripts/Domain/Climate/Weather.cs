using System;
using CifkorTestTask.Domain.Climate.Data;

namespace CifkorTestTask.Domain.Climate
{
    public class Weather
    {
        public event Action<WeatherData> OnWeatherUpdate;
        public event Action<string> OnWeatherError;

        public WeatherData RuntimeData { get; private set; }

        public void SetWeatherData(WeatherData data)
        {
            RuntimeData = data;
            OnWeatherUpdate?.Invoke(data);
        }

        public void SetError(string error) => OnWeatherError?.Invoke(error);
    }
}

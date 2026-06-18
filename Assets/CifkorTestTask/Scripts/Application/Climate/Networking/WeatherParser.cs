using System;
using CifkorTestTask.Domain.Climate.Data;
using UnityEngine;

namespace CifkorTestTask.Application.Climate.Networking
{
    internal static class WeatherParser
    {
        public static WeatherData Parse(string json)
        {
            try
            {
                var root = JsonUtility.FromJson<WeatherApiRoot>(json);
                var period = root?.properties?.periods?[0];
                if (period == null) throw new Exception("No forecast period found");

                return new WeatherData
                {
                    Name = period.name,
                    ShortForecast = period.shortForecast,
                    TemperatureF = period.temperature,
                    IconUrl = period.icon
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"WeatherParser failed: {ex.Message}", ex);
            }
        }

        [Serializable]
        private class WeatherApiRoot
        {
            public WeatherProperties properties;
        }

        [Serializable]
        private class WeatherProperties
        {
            public WeatherPeriod[] periods;
        }

        [Serializable]
        private class WeatherPeriod
        {
            public string name;
            public int temperature;
            public string temperatureUnit;
            public string shortForecast;
            public string icon;
        }
    }
}

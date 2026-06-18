namespace CifkorTestTask.Domain.Climate.Data
{
    [System.Serializable]
    public struct WeatherData
    {
        public string Name;
        public string ShortForecast;
        public int TemperatureF;
        public string IconUrl;
    }
}

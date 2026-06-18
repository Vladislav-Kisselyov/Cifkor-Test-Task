using System.Threading;
using CifkorTestTask.Domain.Climate.Data;
using CifkorTestTask.Infrastructure.Networking;
using Cysharp.Threading.Tasks;

namespace CifkorTestTask.Application.Climate.Networking
{
    internal class WeatherRequest : BaseTaggedRequest<WeatherData>
    {
        public const string WeatherTag = "weather";

        private const string Url = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        private readonly IHttpClient _http;

        public WeatherRequest(IHttpClient http) : base(WeatherTag)
        {
            _http = http;
        }

        protected override async UniTask<WeatherData> ExecuteInternalAsync(CancellationToken ct)
        {
            var json = await _http.GetAsync(Url, ct);
            return WeatherParser.Parse(json);
        }
    }
}

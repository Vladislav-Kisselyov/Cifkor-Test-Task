using System.Threading;
using CifkorTestTask.Infrastructure.Networking;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CifkorTestTask.Presentation.Climate.Networking
{
    internal class WeatherIconRequest : BaseTaggedRequest<Sprite>
    {
        public const string RequestTag = "weather icon";

        private readonly string _url;
        private readonly IHttpClient _http;

        public WeatherIconRequest(string url, IHttpClient http) : base(RequestTag)
        {
            _url = url;
            _http = http;
        }

        protected override UniTask<Sprite> ExecuteInternalAsync(CancellationToken ct)
        {
            return _http.GetSpriteAsync(_url, ct);
        }
    }
}

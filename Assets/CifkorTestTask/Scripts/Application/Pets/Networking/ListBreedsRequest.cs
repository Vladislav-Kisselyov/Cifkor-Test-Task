using System.Threading;
using CifkorTestTask.Domain.Pets.Data;
using CifkorTestTask.Infrastructure.Networking;
using Cysharp.Threading.Tasks;

namespace CifkorTestTask.Application.Pets.Networking
{
    public class ListBreedsRequest : BaseTaggedRequest<BreedData[]>
    {
        public const string RequestTag = "breeds list";
        private const string Url = "https://dogapi.dog/api/v2/breeds?page[number]=1&page[size]=10";
        private readonly IHttpClient _http;

        public ListBreedsRequest(IHttpClient http) : base(RequestTag)
        {
            _http = http;
        }

        protected override async UniTask<BreedData[]> ExecuteInternalAsync(CancellationToken ct)
        {
            var json = await _http.GetAsync(Url, ct);
            return BreedsParser.ParseList(json);
        }
    }
}

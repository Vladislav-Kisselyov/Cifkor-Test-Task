using System.Threading;
using CifkorTestTask.Application.Pets.Networking;
using CifkorTestTask.Domain.Pets.Data;
using CifkorTestTask.Infrastructure.Networking;
using Cysharp.Threading.Tasks;

namespace CifkorTestTask.Presentation.Pets.Networking
{
    public class GetBreedDetailsRequest : BaseTaggedRequest<BreedData>
    {
        public const string RequestTag = "breed details";
        private readonly IHttpClient _http;
        private readonly string _breedId;

        public GetBreedDetailsRequest(string breedId, IHttpClient http)
            : base(RequestTag)
        {
            _breedId = breedId;
            _http = http;
        }

        protected override async UniTask<BreedData> ExecuteInternalAsync(CancellationToken ct)
        {
            var url = $"https://dogapi.dog/api/v2/breeds/{_breedId}";
            var json = await _http.GetAsync(url, ct);
            return BreedsParser.ParseSingle(json);
        }
    }
}

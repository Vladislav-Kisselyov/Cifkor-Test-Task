using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CifkorTestTask.Infrastructure.Networking
{
    public interface IHttpClient
    {
        UniTask<string> GetAsync(string url, CancellationToken cancellationToken = default);
        UniTask<Sprite> GetSpriteAsync(string url, CancellationToken cancellationToken = default);
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CifkorTestTask.Infrastructure.Networking.Internal
{
    internal class UnityHttpClient : IHttpClient
    {
        private const int TimeoutSeconds = 30;

        public async UniTask<string> GetAsync(string url, CancellationToken cancellationToken = default)
        {
            using var request = await GetRequestAsync(url, cancellationToken);
            return request.downloadHandler.text;
        }

        public async UniTask<Sprite> GetSpriteAsync(string url, CancellationToken cancellationToken = default)
        {
            using var request = await GetTextureRequestAsync(url, cancellationToken);
            var tex = DownloadHandlerTexture.GetContent(request);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        }

        private async UniTask<UnityWebRequest> GetRequestAsync(string url, CancellationToken cancellationToken = default)
        {
            var request = UnityWebRequest.Get(url);
            request.timeout = TimeoutSeconds;

            var op = request.SendWebRequest();

            while (!op.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    request.Abort();
                    throw new OperationCanceledException(cancellationToken);
                }
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                request.Abort();
                throw new OperationCanceledException(cancellationToken);
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"HTTP GET {url} failed: {request.error} (code {request.responseCode})");
            }

            return request;
        }

        private async UniTask<UnityWebRequest> GetTextureRequestAsync(
            string url,
            CancellationToken cancellationToken = default)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            request.timeout = TimeoutSeconds;

            var op = request.SendWebRequest();

            while (!op.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception(
                    $"HTTP GET {url} failed: {request.error} (code {request.responseCode})");
            }

            return request;
        }
    }
}

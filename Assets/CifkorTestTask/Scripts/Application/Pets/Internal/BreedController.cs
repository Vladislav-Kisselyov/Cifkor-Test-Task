using System;
using System.Threading;
using CifkorTestTask.Application.Pets.Networking;
using CifkorTestTask.Domain.Pets;
using CifkorTestTask.Domain.Pets.Data;
using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Infrastructure.Networking;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Application.Pets.Internal
{
   internal class BreedController : IBreedController, IInitializable, IDisposable
    {
        private readonly Breed _domain;
        private readonly IRequestQueueService _queue;
        private readonly IInjectionFactory _factory;

        private CancellationTokenSource _lifetimeCts;
        private CancellationTokenSource _requestCts;

        public event Action OnBreedUpdateEnqueue;
        public event Action<BreedData[]> OnBreedUpdate;
        public event Action<string> OnBreedError;

        public BreedController(
            Breed domain,
            IRequestQueueService queue,
            IInjectionFactory factory)
        {
            _domain = domain;
            _queue = queue;
            _factory = factory;
        }

        void IInitializable.Initialize()
        {
            _lifetimeCts = new CancellationTokenSource();

            _domain.OnBreedUpdate += HandleBreedDomainUpdate;
            _domain.OnBreedError += HandleBreedDomainError;
        }

        void IDisposable.Dispose()
        {
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            _requestCts?.Cancel();
            _requestCts?.Dispose();

            _domain.OnBreedUpdate -= HandleBreedDomainUpdate;
            _domain.OnBreedError -= HandleBreedDomainError;
        }

        public void StartRequest()
        {
            _requestCts?.Cancel();
            _requestCts?.Dispose();
            _requestCts = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeCts.Token);
            EnqueueBreedFetch(_requestCts.Token).Forget();
        }

        public void StopRequest()
        {
            _queue.CancelWhere(r => r is BaseTaggedRequest<BreedData[]> { Tag: ListBreedsRequest.RequestTag });
            _requestCts?.Cancel();
            _requestCts?.Dispose();
            _requestCts = null;
        }

        private async UniTaskVoid EnqueueBreedFetch(CancellationToken ct)
        {
            var request = _factory.Create<ListBreedsRequest>();
            try
            {
                OnBreedUpdateEnqueue?.Invoke();
                var data = await _queue.Enqueue(request);

                if (ct.IsCancellationRequested)
                    return;

                _domain.SetBreedData(data);
            }
            catch (OperationCanceledException)
            {
                /* закрылась вкладка или игра */
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                _domain.SetError(ex.Message);
            }
        }

        private void HandleBreedDomainUpdate(BreedData[] data)
        {
            Debug.Log($"[BreedController] Handling breed domain update");
            OnBreedUpdate?.Invoke(data);
        }

        private void HandleBreedDomainError(string error)
        {
            Debug.LogError($"[BreedController] Handling breed domain error: {error}");
            OnBreedError?.Invoke(error);
        }
    }
}

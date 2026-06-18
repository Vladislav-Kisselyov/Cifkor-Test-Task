using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CifkorTestTask.Infrastructure.Networking
{
    public abstract class BaseRequest<TResult> : IRequest
    {
        public string RequestId { get; }
        public RequestState State { get; private set; } = RequestState.Queued;

        private readonly TaskCompletionSource<TResult> _tcs = new();
        private CancellationTokenSource _internalCts;

        public UniTask<TResult> ResultTask => _tcs.Task.AsUniTask();

        protected BaseRequest()
        {
            RequestId = Guid.NewGuid().ToString();
        }

        public async UniTask ExecuteAsync(CancellationToken externalToken)
        {
            if (State == RequestState.Cancelled)
            {
                _tcs.TrySetCanceled();
                return;
            }

            _internalCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            State = RequestState.Executing;

            try
            {
                var result = await ExecuteInternalAsync(_internalCts.Token);
                State = RequestState.Completed;
                _tcs.TrySetResult(result);
            }
            catch (OperationCanceledException)
            {
                State = RequestState.Cancelled;
                _tcs.TrySetCanceled();
            }
            catch (Exception ex)
            {
                State = RequestState.Failed;
                Debug.LogError($"[Request:{RequestId}] Failed: {ex.Message}");
                _tcs.TrySetException(ex);
            }
            finally
            {
                _internalCts?.Dispose();
                _internalCts = null;
            }
        }

        public void Cancel()
        {
            if (State == RequestState.Completed || State == RequestState.Failed)
                return;

            State = RequestState.Cancelled;
            _internalCts?.Cancel();
            _tcs.TrySetCanceled();
        }

        protected abstract UniTask<TResult> ExecuteInternalAsync(CancellationToken cancellationToken);
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Infrastructure.Networking.Internal
{
    internal class RequestQueueService : IRequestQueueService, IInitializable, IDisposable
    {
        private readonly Queue<IRequest> _queue = new();
        private readonly Dictionary<string, IRequest> _requestsById = new();
        private readonly object _lock = new();

        private IRequest _currentRequest;
        private CancellationTokenSource _processingCts;
        private bool _isDisposed;

        public bool IsProcessing => _currentRequest != null;
        public int QueuedCount => _queue.Count;

        void IInitializable.Initialize()
        {
            _processingCts = new CancellationTokenSource();
            ProcessQueueAsync(_processingCts.Token).Forget();
        }

        void IDisposable.Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            _processingCts?.Cancel();
            _processingCts?.Dispose();

            lock (_lock)
            {
                foreach (var req in _queue)
                    req.Cancel();
                _queue.Clear();
                _currentRequest?.Cancel();
                _requestsById.Clear();
            }
        }

        public UniTask<TResult> Enqueue<TResult>(BaseRequest<TResult> request)
        {
            lock (_lock)
            {
                _queue.Enqueue(request);
                _requestsById[request.RequestId] = request;
                Debug.Log($"[Queue] Enqueued {request.RequestId}. Queue size: {_queue.Count}");
            }
            return request.ResultTask;
        }

        public void CancelRequest(string requestId)
        {
            lock (_lock)
            {
                if (!_requestsById.TryGetValue(requestId, out var request))
                    return;

                request.Cancel();
                _requestsById.Remove(requestId);
                Debug.Log($"[Queue] Cancelled request {requestId}");
            }
        }

        public void CancelWhere(Func<IRequest, bool> predicate)
        {
            lock (_lock)
            {
                if (_currentRequest != null && predicate(_currentRequest))
                {
                    _currentRequest.Cancel();
                    _requestsById.Remove(_currentRequest.RequestId);
                }

                var remaining = new Queue<IRequest>();
                while (_queue.Count > 0)
                {
                    var req = _queue.Dequeue();
                    if (predicate(req))
                    {
                        req.Cancel();
                        _requestsById.Remove(req.RequestId);
                        Debug.Log($"[Queue] Removed queued request {req.RequestId}");
                    }
                    else
                    {
                        remaining.Enqueue(req);
                    }
                }

                while (remaining.Count > 0)
                    _queue.Enqueue(remaining.Dequeue());
            }
        }

        private async UniTaskVoid ProcessQueueAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                IRequest next = null;

                lock (_lock)
                {
                    if (_queue.Count > 0)
                    {
                        next = _queue.Dequeue();
                        _currentRequest = next;
                    }
                }

                if (next == null)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                    continue;
                }

                try
                {
                    if (next.State != RequestState.Cancelled)
                    {
                        Debug.Log($"[Queue] Executing {next.RequestId}");
                        await next.ExecuteAsync(ct);
                        Debug.Log($"[Queue] Finished {next.RequestId} with state {next.State}");
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    Debug.LogError($"[Queue] Unhandled error in request {next.RequestId}: {ex}");
                }
                finally
                {
                    lock (_lock)
                    {
                        _requestsById.Remove(next.RequestId);
                        if (_currentRequest == next)
                            _currentRequest = null;
                    }
                }
            }
        }
    }
}

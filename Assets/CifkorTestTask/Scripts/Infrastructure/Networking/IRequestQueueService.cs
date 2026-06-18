using System;
using Cysharp.Threading.Tasks;

namespace CifkorTestTask.Infrastructure.Networking
{
    public interface IRequestQueueService
    {
        public bool IsProcessing { get; }
        public int QueuedCount { get; }

        public UniTask<TResult> Enqueue<TResult>(BaseRequest<TResult> request);
        public void CancelRequest(string requestId);
        public void CancelWhere(Func<IRequest, bool> predicate);
    }
}

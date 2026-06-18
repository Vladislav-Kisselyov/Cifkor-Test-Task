using System.Threading;
using Cysharp.Threading.Tasks;

namespace CifkorTestTask.Infrastructure.Networking
{
    public interface IRequest
    {
        string RequestId { get; }
        RequestState State { get; }
        UniTask ExecuteAsync(CancellationToken cancellationToken);
        void Cancel();
    }

    public enum RequestState
    {
        Queued,
        Executing,
        Completed,
        Cancelled,
        Failed
    }
}

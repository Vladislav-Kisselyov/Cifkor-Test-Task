namespace CifkorTestTask.Infrastructure.Networking
{
    public abstract class BaseTaggedRequest<TResult> : BaseRequest<TResult>
    {
        public string Tag { get; }

        protected BaseTaggedRequest(string tag)
        {
            Tag = tag;
        }
    }
}

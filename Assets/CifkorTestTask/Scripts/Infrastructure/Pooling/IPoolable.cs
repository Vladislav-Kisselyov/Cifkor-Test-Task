namespace CifkorTestTask.Infrastructure.Pooling
{
    public interface IPoolable
    {
        public bool IsPoolingEnabled => true;
        public void OnSpawned() {}
        public void OnDespawned() {}
        public void OnPrewarmed() {}
    }
}

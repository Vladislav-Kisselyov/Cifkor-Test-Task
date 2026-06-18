using UnityEngine;

namespace CifkorTestTask.Infrastructure.Pooling
{
    public interface IPoolableFactory
    {
        public TObject Spawn<TObject>(TObject prefab, Transform parent)
            where TObject : MonoBehaviour, IPoolable;

        public void Despawn<TObject>(TObject instance)
            where TObject : MonoBehaviour, IPoolable;

        public TObject Spawn<TObject>(string poolId, TObject prefab, Transform parent)
            where TObject : MonoBehaviour, IPoolable;

        public void Despawn<TObject>(string poolId, TObject instance)
            where TObject : MonoBehaviour, IPoolable;

        void PrewarmPool<TObject>(TObject prefab, int count)
            where TObject : MonoBehaviour, IPoolable;

        void PrewarmPool<TObject>(string poolId, TObject prefab, int count)
            where TObject : MonoBehaviour, IPoolable;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CifkorTestTask.Infrastructure.Pooling.Internal
{
    internal class PoolableFactory : IPoolableFactory, IInitializable
    {
        private readonly Dictionary<Type, Queue<MonoBehaviour>> _poolsByType = new();
        private readonly Dictionary<string, Queue<MonoBehaviour>> _poolsById = new();

        private Transform _poolRoot;

        void IInitializable.Initialize()
        {
            var go = new GameObject("PoolRoot", typeof(RectTransform));
            _poolRoot = go.transform;
            Object.DontDestroyOnLoad(go);
        }

        public TObject Spawn<TObject>(TObject prefab, Transform parent)
            where TObject : MonoBehaviour, IPoolable
        {
            var prefabInstance = ReturnExistingInstanceOrCreateNew(prefab, parent);
            HandlePrefabSpawned(prefabInstance, parent);

            return prefabInstance;
        }

        public void Despawn<TObject>(TObject instance)
            where TObject : MonoBehaviour, IPoolable
        {
            if (instance == null || !instance)
                return;

            if (!instance.IsPoolingEnabled)
            {
                Object.Destroy(instance.gameObject);
                return;
            }

            var type = instance.GetType();
            if (!_poolsByType.TryGetValue(type, out var queue))
            {
                queue = new Queue<MonoBehaviour>();
                _poolsByType[type] = queue;
            }

            queue.Enqueue(instance);

            instance.OnDespawned();
            instance.transform.SetParent(_poolRoot);
        }

        public TObject Spawn<TObject>(string poolId, TObject prefab, Transform parent)
            where TObject : MonoBehaviour, IPoolable
        {
            var prefabInstance = ReturnExistingInstanceOrCreateNew(prefab, parent, poolId);
            HandlePrefabSpawned(prefabInstance, parent);

            return prefabInstance;
        }

        public void Despawn<TObject>(string poolId, TObject instance)
            where TObject : MonoBehaviour, IPoolable
        {
            if (instance == null || !instance)
                return;

            if (!instance.IsPoolingEnabled)
            {
                Object.Destroy(instance.gameObject);
                return;
            }

            if (!_poolsById.TryGetValue(poolId, out var queue))
            {
                queue = new Queue<MonoBehaviour>();
                _poolsById[poolId] = queue;
            }

            queue.Enqueue(instance);

            instance.OnDespawned();
            instance.transform.SetParent(_poolRoot);
        }

        public void PrewarmPool<TObject>(TObject prefab, int count)
            where TObject : MonoBehaviour, IPoolable
        {
            for (int i = 0; i < count; i++)
            {
                var instance = Object.Instantiate(prefab);
                instance.OnPrewarmed();
                Despawn(instance);
            }
        }

        public void PrewarmPool<TObject>(string poolId, TObject prefab, int count)
            where TObject : MonoBehaviour, IPoolable
        {
            for (int i = 0; i < count; i++)
            {
                var instance = Object.Instantiate(prefab);
                instance.OnPrewarmed();
                Despawn(poolId, instance);
            }
        }

        private TObject ReturnExistingInstanceOrCreateNew<TObject>(TObject prefab, Transform parent,
            string poolId = null)
            where TObject : MonoBehaviour, IPoolable
        {
            if (poolId != null && _poolsById.TryGetValue(poolId, out var queue) && queue.Count > 0)
            {
                while (queue.Count > 0)
                {
                    var queuedInstance = queue.Dequeue();

                    if (queuedInstance != null && queuedInstance)
                        return (TObject)queuedInstance;
                }
            }

            if (_poolsByType.TryGetValue(typeof(TObject), out queue) && queue.Count > 0)
            {
                while (queue.Count > 0)
                {
                    var queuedInstance = queue.Dequeue();

                    if (queuedInstance != null && queuedInstance)
                        return (TObject)queuedInstance;
                }
            }

            var newInstance = Object.Instantiate(prefab, parent);
            return newInstance;
        }

        private void HandlePrefabSpawned<TObject>(TObject instance, Transform parent)
            where TObject : MonoBehaviour, IPoolable
        {
            if (parent != null & !parent)
            {
                Debug.LogError($"[Pool] Spawn got DESTROYED parent. prefab={typeof(TObject).Name} parent={(parent ? parent.name : "null")}");
                return;
            }

            if (instance == null)
            {
                Debug.LogError($"[Pool] Spawn got NULL instance. prefab={typeof(TObject).Name} parent={(parent ? parent.name : "null")}");
                return;
            }

            if (!instance)
            {
                Debug.LogError($"[Pool] Spawn got DESTROYED instance. prefab={typeof(TObject).Name} parent={(parent ? parent.name : "null")}");
                return;
            }

            instance.transform.SetParent(parent);

            try
            {
                instance.OnSpawned();
            }
            catch (Exception e)
            {
                Debug.LogError($"[Pool] OnSpawned exception on {instance.name} ({instance.GetType().Name})\n{e}");
                throw;
            }
        }
    }
}

using UnityEngine;

namespace CifkorTestTask.Infrastructure.Configs
{
    public abstract class BaseScriptableObjectConfig<T> : ScriptableObject
    {
        [SerializeField]
        private T _config;

        public T Config => _config;

        public void SetConfig(T value)
        {
            _config = value;
        }
    }
}

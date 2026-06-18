using System;
using Zenject;

namespace CifkorTestTask.Infrastructure.Injection.Internal
{
    public class ZenjectFactory : IInjectionFactory
    {
        private readonly DiContainer _container;

        public ZenjectFactory(DiContainer container)
        {
            _container = container;
        }

        public TResult Create<TResult, T>(T param)
        {
            var newObject = _container.Instantiate<TResult>(
                new object[] { param });

            if (newObject is IInitializable initializableObject)
                initializableObject.Initialize();

            return newObject;
        }

        public TResult Create<TResult, T1, T2>(T1 param1, T2 param2)
        {
            var newObject = _container.Instantiate<TResult>(
                new object[] { param1, param2 });

            if (newObject is IInitializable initializableObject)
                initializableObject.Initialize();

            return newObject;
        }

        public TResult Create<TResult, T1, T2, T3>(
            T1 param1,
            T2 param2,
            T3 param3)
        {
            var newObject = _container.Instantiate<TResult>(
                new object[] { param1, param2, param3 });

            if (newObject is IInitializable initializableObject)
                initializableObject.Initialize();

            return newObject;
        }

        public TResult Create<TResult>(params object[] parameters)
        {
            var newObject = _container.Instantiate<TResult>(parameters);

            if (newObject is IInitializable initializableObject)
                initializableObject.Initialize();

            return newObject;
        }

        public object Create(Type type, params object[] parameters)
        {
            var newObject = _container.Instantiate(type, parameters);

            if (newObject is IInitializable initializableObject)
                initializableObject.Initialize();

            return newObject;
        }

        public TResult Inject<TResult>(TResult instance)
        {
            _container.Inject(instance);

            if (instance is IInitializable initializableObject)
                initializableObject.Initialize();

            return instance;
        }
    }
}

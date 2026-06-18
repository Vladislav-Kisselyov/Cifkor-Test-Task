using System;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Application.Pets.Internal
{
    internal class BreedController : IBreedController, IInitializable, IDisposable
    {
        void IInitializable.Initialize()
        {
            Debug.Log("Initialized");
        }

        void IDisposable.Dispose()
        {
            Debug.Log("Disposed");
        }
    }
}

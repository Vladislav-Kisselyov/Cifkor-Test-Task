using System;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Pets.Presenter
{
    public class BreedPresenter : IInitializable, IDisposable
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

using System;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Cookie.Presenter
{
    public class ClickerPresenter : IInitializable, IDisposable
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

using System;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Application.Cookie.Internal
{
    internal class ClickerController : IClickerController, IInitializable, IDisposable
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

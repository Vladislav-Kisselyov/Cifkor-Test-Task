using System;

namespace CifkorTestTask.Application.Cookie
{
    public interface IClickerController
    {
        public event Action<int, int> OnCurrencyChanged;
        public event Action<int> OnEnergyChanged;
        public event Action OnTapExecuted;
        public event Action OnAutoCollectExecuted;
        public int Currency { get; }
        public int Energy { get; }
        public void TryTap();
    }
}

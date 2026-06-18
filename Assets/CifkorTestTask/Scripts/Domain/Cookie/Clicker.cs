using System;
using CifkorTestTask.Domain.Cookie.Config;
using CifkorTestTask.Domain.Cookie.Data;

namespace CifkorTestTask.Domain.Cookie
{
    public class Clicker    {

        private readonly ClickerConfig _config;
        private ClickerData _runtimeData;

        public ClickerData RuntimeData => _runtimeData;
        public event Action<int> OnCurrencyChanged;
        public event Action<int> OnEnergyChanged;
        public event Action OnTapExecuted;
        public event Action OnAutoCollectExecuted;

        public Clicker(ClickerConfig config)
        {
            _config = config;
            _runtimeData.Energy = config.MaxEnergy;
        }

        public bool TryTap()
        {
            if (_runtimeData.Energy < _config.EnergyCostPerTap)
                return false;

            SpendEnergy(_config.EnergyCostPerTap);
            AddCurrency(_config.CurrencyPerTap);
            OnTapExecuted?.Invoke();
            return true;
        }

        public bool TryAutoCollect()
        {
            if (_runtimeData.Energy < _config.EnergyCostPerAutoCollect)
                return false;

            SpendEnergy(_config.EnergyCostPerAutoCollect);
            AddCurrency(_config.CurrencyPerAutoCollect);
            OnAutoCollectExecuted?.Invoke();
            return true;
        }

        public void RegenEnergy()
        {
            var newEnergy = Math.Min(_runtimeData.Energy + _config.EnergyRegenAmount, _config.MaxEnergy);
            if (newEnergy == _runtimeData.Energy)
                return;

            _runtimeData.Energy = newEnergy;
            OnEnergyChanged?.Invoke(_runtimeData.Energy);
        }

        private void AddCurrency(int amount)
        {
            _runtimeData.Currency += amount;
            OnCurrencyChanged?.Invoke(_runtimeData.Currency);
        }

        private void SpendEnergy(int amount)
        {
            _runtimeData.Energy = Math.Max(0, _runtimeData.Energy - amount);
            OnEnergyChanged?.Invoke(_runtimeData.Energy);
        }
    }
}

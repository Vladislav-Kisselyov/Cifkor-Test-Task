using System;
using UnityEngine;

namespace CifkorTestTask.Domain.Cookie.Config
{
    [Serializable]
    public class ClickerConfig
    {
        [SerializeField] private int _currencyPerTap = 1;
        [SerializeField] private int _currencyPerAutoCollect = 1;
        [SerializeField] private int _maxEnergy = 1000;
        [SerializeField] private int _energyCostPerTap = 1;
        [SerializeField] private int _energyCostPerAutoCollect = 1;
        [SerializeField] private int _energyRegenAmount = 10;
        [SerializeField] private float _energyRegenIntervalSeconds = 10f;
        [SerializeField] private float _autoCollectIntervalSeconds = 3f;

        public int CurrencyPerTap => _currencyPerTap;
        public int CurrencyPerAutoCollect => _currencyPerAutoCollect;
        public int MaxEnergy => _maxEnergy;
        public int EnergyCostPerTap => _energyCostPerTap;
        public int EnergyCostPerAutoCollect => _energyCostPerAutoCollect;
        public int EnergyRegenAmount => _energyRegenAmount;
        public float EnergyRegenIntervalSeconds => _energyRegenIntervalSeconds;
        public float AutoCollectIntervalSeconds => _autoCollectIntervalSeconds;
    }
}

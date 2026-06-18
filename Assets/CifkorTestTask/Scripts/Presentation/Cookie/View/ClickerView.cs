using System;
using CifkorTestTask.Presentation.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CifkorTestTask.Presentation.Cookie.View
{
    public interface IClickerView : IViewBase
    {
        public event Action OnButtonClick;

        public void SetCurrency(int newCurrencyValue);
        public void SetEnergy(int newEnergyValue);
    }

    public class ClickerView : ViewBase, IClickerView
    {
        [Header("Component References")]

        [SerializeField] private TMP_Text _currencyLabel;
        [SerializeField] private TMP_Text _energyLabel;

        [SerializeField] private Button _button;

        public event Action OnButtonClick;

        private void Awake()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetCurrency(int newCurrencyValue)
        {
            _currencyLabel.SetText(newCurrencyValue.ToString());
        }

        public void SetEnergy(int newEnergyValue)
        {
            _energyLabel.SetText(newEnergyValue.ToString());
        }

        private void HandleButtonClick()
        {
            OnButtonClick?.Invoke();
        }
    }
}

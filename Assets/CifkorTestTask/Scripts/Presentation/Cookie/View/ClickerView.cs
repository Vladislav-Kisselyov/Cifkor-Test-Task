using System;
using BrunoMikoski.AnimationSequencer;
using CifkorTestTask.Presentation.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CifkorTestTask.Presentation.Cookie.View
{
    public interface IClickerView : IViewBase
    {
        public event Action OnButtonClick;

        public Sprite CurrencySprite { get; }
        public Transform CurrencyTransform { get; }
        public Transform ButtonTransform { get; }


        public void SetCurrency(int newCurrencyValue);
        public void SetEnergy(int newEnergyValue);
        public void PlayButtonBounceAnimation();
    }

    public class ClickerView : ViewBase, IClickerView
    {
        [Header("Additional Animations")]
        [SerializeField] private AnimationSequencerController _bounceAnimation;

        [Header("Component References")]
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TMP_Text _currencyLabel;
        [SerializeField] private TMP_Text _energyLabel;
        [SerializeField] private Button _button;

        public Sprite CurrencySprite => _currencyImage.sprite;
        public Transform CurrencyTransform => _currencyImage.transform;
        public Transform ButtonTransform => _button.transform;

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

        public void PlayButtonBounceAnimation()
        {
            _bounceAnimation.Kill(true);
            _bounceAnimation.Play();
        }

        private void HandleButtonClick()
        {
            OnButtonClick?.Invoke();
        }
    }
}

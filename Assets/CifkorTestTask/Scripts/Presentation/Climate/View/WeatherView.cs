using System;
using CifkorTestTask.Presentation.Screens;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CifkorTestTask.Presentation.Climate.View
{
    public interface IWeatherView : IViewBase, IDisposable
    {
        public void SetLoading(bool isLoadingShown);
        public void SetText(string label);
        public void SetIcon(Sprite icon);
        public void SetIconDefault();
        public void ShowError(string message);
    }

    internal class WeatherView : ViewBase, IWeatherView
    {
        [Header("Component References")]
        [SerializeField] private GameObject _loadingIndicator;
        [SerializeField] private Image _weatherIcon;
        [SerializeField] private TMP_Text _weatherLabel;
        [SerializeField] private TMP_Text _errorLabel;

        [Header("Asset References")]
        [SerializeField] private Sprite _defaultSprite;

        private Sequence _errorSequence;

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            _errorSequence?.Kill();
        }

        public void SetLoading(bool isLoadingShown)
        {
            _loadingIndicator.SetActive(isLoadingShown);
        }

        public void SetText(string label)
        {
            _weatherLabel.text = label;
        }

        public void SetIcon(Sprite icon)
        {
            _weatherIcon.sprite = icon;
        }

        public void SetIconDefault()
        {
            _weatherIcon.sprite = _defaultSprite;
        }

        public void ShowError(string message)
        {
            _errorSequence?.Kill();
            _errorSequence = DOTween.Sequence();

            _errorSequence.Append(_errorLabel.DOFade(1f, 0f));
            _errorSequence.AppendCallback(() => _errorLabel.SetText(string.Empty));
            _errorSequence.AppendCallback(() => _errorLabel.gameObject.SetActive(true));
            _errorSequence.Append(_errorLabel.DOText(message, 1f));
            _errorSequence.AppendInterval(4f);
            _errorSequence.Append(_errorLabel.DOFade(0f, 1f));
            _errorSequence.AppendCallback(() => _errorLabel.gameObject.SetActive(false));
        }
    }
}

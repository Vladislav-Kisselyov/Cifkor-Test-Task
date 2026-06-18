using System;
using CifkorTestTask.Presentation.Screens;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CifkorTestTask.Presentation.Pets.View
{
    public interface IBreedView : IViewBase, IDisposable
    {
        public BreedInfoView BreedInfoPrefab { get; }
        public Transform ScrollPanelContentParent { get; }
        public void SetLoading(bool isLoadingShown);
        public void ShowError(string message);
    }

    internal class BreedView : ViewBase, IBreedView
    {
        [Header("Component References")]
        [SerializeField] private GameObject _loadingIndicator;
        [SerializeField] private TMP_Text _errorLabel;
        [SerializeField] private Transform _scrollPanelContentParent;
        [SerializeField] private BreedInfoView _breedInfoPrefab;

        private Sequence _errorSequence;

        public BreedInfoView BreedInfoPrefab => _breedInfoPrefab;
        public Transform ScrollPanelContentParent => _scrollPanelContentParent;

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

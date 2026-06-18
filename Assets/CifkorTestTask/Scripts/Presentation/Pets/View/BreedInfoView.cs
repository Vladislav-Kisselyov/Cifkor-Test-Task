using System;
using CifkorTestTask.Infrastructure.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CifkorTestTask.Presentation.Pets.View
{
    public interface IBreedInfoView : IPoolable
    {
        public event Action OnClick;
        public void SetText(string breedIndex, string breedName);
        public void SetLoading(bool isLoading);
    }

    public class BreedInfoView : MonoBehaviour, IBreedInfoView
    {
        [SerializeField] private TMP_Text _breedIndex;
        [SerializeField] private TMP_Text _breedName;
        [SerializeField] private GameObject _loadingIndicator;
        [SerializeField] private Button _button;

        public event Action OnClick;

        private void Awake()
        {
            _button.onClick.AddListener(HandleButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void HandleButtonClicked()
        {
            OnClick?.Invoke();
        }

        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }

        public void SetText(string breedIndex, string breedName)
        {
            _breedIndex.SetText(breedIndex);
            _breedName.SetText(breedName);
        }

        public void SetLoading(bool isLoading)
        {
            _loadingIndicator.SetActive(isLoading);
        }
    }
}

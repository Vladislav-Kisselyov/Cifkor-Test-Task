using System;
using CifkorTestTask.Presentation.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CifkorTestTask.Presentation.Pets.View
{
    public interface IBreedDetailsView : IViewBase
    {
        public event Action OnExitButtonClick;
        public void SetText(string breedName, string breedDescription);
    }

    public class BreedDetailsView: ViewBase, IBreedDetailsView
    {
        [SerializeField] private TMP_Text _breedName;
        [SerializeField] private TMP_Text _breedDescription;
        [SerializeField] private Button _exitButton;

        public event Action OnExitButtonClick;

        private void Awake()
        {
            _exitButton.onClick.AddListener(HandleExitButtonClicked);
        }

        private void OnDestroy()
        {
            _exitButton.onClick.RemoveAllListeners();
        }

        private void HandleExitButtonClicked()
        {
            OnExitButtonClick?.Invoke();
        }

        public void SetText(string breedName, string breedDescription)
        {
            _breedName.SetText(breedName);
            _breedDescription.SetText(breedDescription);
        }
    }
}

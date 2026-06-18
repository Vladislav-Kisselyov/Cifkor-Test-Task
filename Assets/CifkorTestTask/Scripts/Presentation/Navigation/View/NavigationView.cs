using System;
using CifkorTestTask.Infrastructure.Collections;
using CifkorTestTask.Presentation.Screens;
using UnityEngine;

namespace CifkorTestTask.Presentation.Navigation.View
{
    public interface INavigationView : IViewBase
    {
        public event Action<int> OnButtonClicked;
        public void SetButtonActive(int buttonIndex);
    }

    internal class NavigationView : ViewBase, INavigationView
    {
        [Header("Component References")]
        [SerializeField] private UDictionary<int, NavigationButtonView> _buttons;

        public event Action<int> OnButtonClicked;

        private void Awake()
        {
            foreach (var kvp in _buttons)
            {
                var key = kvp.Key;
                var button = kvp.Value;

                button.SetOnClickCallback(key, HandleAnyButtonClicked);
            }
        }

        private void HandleAnyButtonClicked(int buttonIndex)
        {
            OnButtonClicked?.Invoke(buttonIndex);
        }

        public void SetButtonActive(int buttonIndex)
        {
            foreach (var kvp in _buttons)
            {
                kvp.Value.SetActive(kvp.Key == buttonIndex);
            }
        }
    }
}

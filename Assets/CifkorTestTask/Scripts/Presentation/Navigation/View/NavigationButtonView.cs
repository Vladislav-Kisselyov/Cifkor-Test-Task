using System;
using BrunoMikoski.AnimationSequencer;
using UnityEngine;
using UnityEngine.UI;

namespace CifkorTestTask.Presentation.Navigation.View
{
    public class NavigationButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AnimationSequencerController _toActiveAnimation;
        [SerializeField] private AnimationSequencerController _toInactiveAnimation;

        private int _buttonIndex;
        private Action<int> _onClick;

        private void Awake()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetOnClickCallback(int buttonIndex, Action<int> onClick)
        {
            _buttonIndex = buttonIndex;
            _onClick = onClick;
        }

        public void SetActive(bool isActive, bool animate = true)
        {
            var toAnimation = isActive
                ? _toActiveAnimation
                : _toInactiveAnimation;

            toAnimation.Play();

            if (animate == false)
                toAnimation.Kill(true);
        }

        private void HandleButtonClick()
        {
            _onClick.Invoke(_buttonIndex);
        }
    }
}

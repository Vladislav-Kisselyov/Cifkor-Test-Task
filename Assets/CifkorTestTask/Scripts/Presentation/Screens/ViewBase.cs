using BrunoMikoski.AnimationSequencer;
using UnityEngine;

namespace CifkorTestTask.Presentation.Screens
{
    public abstract class ViewBase : MonoBehaviour, IViewBase
    {
        [Header("Base Animation References")]
        [SerializeField] private AnimationSequencerController _showAnimationSequencer;
        [SerializeField] private AnimationSequencerController _hideAnimationSequencer;

        public void Show(bool withAnimation = true)
        {
            Debug.Log($"{GetType().Name}: Show()");
            _hideAnimationSequencer?.Kill();

            gameObject.SetActive(true);

            if (_showAnimationSequencer != null)
            {
                _showAnimationSequencer.Play();
                if (!withAnimation)
                    _showAnimationSequencer.Kill(false);
            }
        }

        public void Hide(bool withAnimation = true)
        {
            Debug.Log($"{GetType().Name}: Hide()");
            if (_hideAnimationSequencer != null && withAnimation)
            {
                _hideAnimationSequencer.Play(() => gameObject.SetActive(false));
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            TryAssignSequencer(
                ref _showAnimationSequencer,
                "BaseAnimation/ShowAnimation");

            TryAssignSequencer(
                ref _hideAnimationSequencer,
                "BaseAnimation/HideAnimation");
        }

        private void TryAssignSequencer(
            ref AnimationSequencerController field,
            string relativePath)
        {
            if (field != null)
                return;

            var target = transform.Find(relativePath);

            if (target == null)
                return;

            field = target.GetComponent<AnimationSequencerController>();
        }
#endif
    }
}

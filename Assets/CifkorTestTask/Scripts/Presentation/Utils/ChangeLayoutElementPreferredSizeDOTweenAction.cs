using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CifkorTestTask.Presentation.Utils
{
    [Serializable]
    public class ChangeLayoutElementPreferredSizeDOTweenAction : DOTweenActionBase
    {
        [SerializeField]
        private Vector2 _size;

        private Vector2 _initialSize;

        private LayoutElement _layoutElement;

        public override string DisplayName => "Layout Element Preferred Size";

        public override Type TargetComponentType => typeof(LayoutElement);

        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            if (_layoutElement == null)
            {
                _layoutElement = target.GetComponent<LayoutElement>();

                if (_layoutElement == null)
                {
                    Debug.LogError($"{target} does not have {TargetComponentType} component");
                    return null;
                }
            }

            _initialSize = new Vector2(_layoutElement.preferredWidth, _layoutElement.preferredHeight);

            return _layoutElement.DOPreferredSize(_size, duration);
        }

        public override void ResetToInitialState()
        {
            if (_layoutElement == null)
            {
                return;
            }

            _layoutElement.preferredWidth = _initialSize.x;
            _layoutElement.preferredHeight = _initialSize.y;
        }
    }
}

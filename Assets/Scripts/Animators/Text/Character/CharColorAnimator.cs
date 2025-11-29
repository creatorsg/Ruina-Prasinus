using UnityEngine;
using DG.Tweening;

namespace Runtime.Utilities.Animators.Text.Character
{
    public class CharColorAnimator : AbstractCharAnimator
    {
        [Tooltip("애니메이션 목표값입니다.")]
        [SerializeField] private Color _targetValue;
        [Tooltip("애니메이션 지속 시간(초)입니다.")]
        [SerializeField] private float _duration = 0.5f;
        [Tooltip("적용할 이징(Ease) 타입입니다.")]
        [SerializeField] private Ease _ease = Ease.Linear;
        /// <summary>
        /// 문자 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateCharAnimationSequence(int index)
        {
            Sequence sequence = DOTween.Sequence()
            .Append(Animator.DOColorChar(index, _targetValue, _duration))
            .SetEase(_ease);

            return sequence;
        }
    }
}
using UnityEngine;
using DG.Tweening;

namespace Runtime.Utilities.Animators.Text.Character
{
    public class ShakeCharScaleAnimator : AbstractCharAnimator
    {
        [Tooltip("애니메이션 목표값입니다.")]
        [SerializeField] private Vector3 _targetValue = new(0.25f, 0.25f, 0.25f);
        [Tooltip("애니메이션 지속 시간(초)입니다.")]
        [SerializeField] private float _duration = 1f;
        [Tooltip("초당 진동 횟수입니다.")]
        [SerializeField] private int _vibrato = 5;
        [Tooltip("진동 방향의 무작위성 정도(0~180)입니다.")]
        [SerializeField, Range(0f, 180f)] private float _randomness = 90;
        [Tooltip("활성화 시 진동이 점점 줄어들도록 처리합니다.")]
        [SerializeField] private bool _fadeOut = true;
        [Tooltip("적용할 이징(Ease) 타입입니다.")]
        [SerializeField] private Ease _ease = Ease.Linear;
        /// <summary>
        /// 문자 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateCharAnimationSequence(int index)
        {
            Sequence sequence = DOTween.Sequence()
            .Append(Animator.DOShakeCharScale(index, _duration, _targetValue, _vibrato, _randomness, _fadeOut))
            .SetEase(_ease);

            return sequence;
        }
    }
}
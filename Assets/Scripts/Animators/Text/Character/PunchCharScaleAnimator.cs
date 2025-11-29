using UnityEngine;
using DG.Tweening;

namespace Runtime.Utilities.Animators.Text.Character
{
    public class PunchCharScaleAnimator : AbstractCharAnimator
    {
        [Tooltip("애니메이션 목표값입니다.")]
        [SerializeField] private Vector3 _targetValue = new(0.25f, 0.25f, 0.25f);
        [Tooltip("애니메이션 지속 시간(초)입니다.")]
        [SerializeField] private float _duration = 1f;
        [Tooltip("초당 진동 횟수입니다.")]
        [SerializeField] private int _vibrato = 5;
        [Tooltip("얼마나 튕겨서 반대 방향까지 진동할지를 0~1 사이로 설정합니다.\n" +
                 "1에 가까울수록 원래 크기보다 더 작아졌다가 다시 커지며, 완전한 진동을 만듭니다.\n" +
                 "0에 가까울수록 반대 방향으로 넘어가지 않고 원래 크기까지만 돌아옵니다.")]
        [SerializeField, Range(0f, 1f)] private float _elasticity = 1f;
        [Tooltip("적용할 이징(Ease) 타입입니다.")]
        [SerializeField] private Ease _ease = Ease.Linear;
        /// <summary>
        /// 문자 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateCharAnimationSequence(int index)
        {
            Sequence sequence = DOTween.Sequence()
            .Append(Animator.DOPunchCharScale(index, _targetValue, _duration, _vibrato, _elasticity))
            .SetEase(_ease);

            return sequence;
        }
    }
}
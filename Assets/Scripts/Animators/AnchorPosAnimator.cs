using UnityEngine;
using DG.Tweening;

namespace Runtime.Utilities.Animators
{
    public class AnchorPosAnimator : AbstractAnimator
    {
        [Tooltip("애니메이션 대상입니다.")]
        public RectTransform Target;
        [Tooltip("상대 위치로 이동할지 여부를 결정합니다. 활성화하면 현재 위치를 기준으로 지정된 거리만큼 이동합니다.")]
        public bool Relative = false;
        [Tooltip("이동할 위치 또는 거리입니다. 상대 위치 이동이 활성화된 경우, 현재 위치에서의 거리로 사용됩니다.")]
        public Vector3 EndValue = Vector3.zero;
        [Tooltip("애니메이션의 지속 시간(초)입니다.")]
        public float Duration = 0.5f;
        [Tooltip("좌표 값을 정수로 강제할지 여부를 결정합니다.")]
        public bool Snapping = false;
        [Tooltip("적용할 이징(Ease) 타입입니다.")]
        public Ease Ease = Ease.Linear;
        /// <summary>
        /// 위치 이동 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateAnimationSequence()
        {
            return DOTween.Sequence()
            .Append(Target.DOAnchorPos(EndValue, Duration, Snapping).SetEase(Ease).SetRelative(Relative));
        }
    }
}
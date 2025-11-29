using UnityEngine;
using DG.Tweening;

namespace Runtime.Utilities.Animators
{
    public class PunchAnimator : AbstractAnimator
    {
        [Tooltip("애니메이션 대상 오브젝트입니다.")]
        public Transform Target;
        [Tooltip("상대 크기로 조절할지 여부를 결정합니다. 활성화하면 현재 크기를 기준으로 지정된 크기만큼 조절합니다.")]
        public bool Relative = false;
        [Tooltip("펀치 크기 값입니다.")]
        public Vector3 Punch = Vector3.zero;
        [Tooltip("애니메이션의 지속 시간(초)입니다.")]
        public float Duration = 0.5f;
        [Tooltip("초당 진동 횟수입니다.")]
        public int Vibrato = 50;
        [Tooltip("얼마나 튕겨서 반대 방향까지 진동할지를 0~1 사이로 설정합니다.\n" +
                 "1에 가까울수록 원래 크기보다 더 작아졌다가 다시 커지며, 완전한 진동을 만듭니다.\n" +
                 "0에 가까울수록 반대 방향으로 넘어가지 않고 원래 크기까지만 돌아옵니다.")]
        [SerializeField, Range(0f, 1f)] public float Elasticity = 1f;
        [Tooltip("적용할 이징(Ease) 타입입니다.")]
        public Ease Ease = Ease.Linear;
        /// <summary>
        /// 크기 변환 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateAnimationSequence()
        {
            return DOTween.Sequence()
            .Append(Target.DOPunchScale(Punch, Duration, Vibrato, Elasticity).SetEase(Ease).SetRelative(Relative));
        }
    }
}
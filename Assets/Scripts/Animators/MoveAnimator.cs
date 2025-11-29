using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Runtime.Utilities.Animators
{
    public class MoveAnimator : AbstractAnimator
    {
        [Tooltip("애니메이션 대상 오브젝트입니다.")]
        public Transform Target;
        [Tooltip("상대 위치로 이동할지 여부를 결정합니다. 활성화하면 현재 위치를 기준으로 지정된 거리만큼 이동합니다.")]
        public bool Relative = false;
        [Tooltip("애니메이션 시작 시 대상값을 초기값으로 설정할지 여부를 결정합니다.")]
        public bool ApplyInitialValue = false;
        [ShowIf(nameof(ApplyInitialValue))]
        [Tooltip("애니메이션 시작 시 적용할 초기값입니다.")]
        public Vector3 InitialValue = Vector3.zero;
        [Tooltip("이동할 위치 또는 거리입니다. 상대 위치 이동이 활성화된 경우, 현재 위치에서의 거리로 사용됩니다.")]
        public Vector3 EndValue = Vector3.zero;
        [Tooltip("애니메이션의 지속 시간(초)입니다.")]
        public float Duration = 0.5f;
        [Tooltip("좌표 값을 정수로 강제할지 여부를 결정합니다.")]
        public bool Snapping = false;
        [Tooltip("적용할 이징(Ease) 타입입니다.")]
        public Ease Ease = Ease.Linear;
        [Tooltip("애니메이션 시작 시, 초기값을 시작과 동시에 초기화 할지 여부를 결정합니다.")]
        public bool ResetWithInitial = true;
        /// <summary>
        /// 시작 시 설정 여부에 따라 대상값을 초기값으로 설정합니다.
        /// </summary>
        protected override void OnCreate()
        {
            if (ResetWithInitial)
            {
                if (ApplyInitialValue)
                {
                    Target.localPosition = InitialValue;
                }
            }
        }
        protected override void OnStart()
        {
            if (!ResetWithInitial)
            {
                if (ApplyInitialValue)
                {
                    Target.localPosition = InitialValue;
                }
            }
        }
        /// <summary>
        /// 위치 이동 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateAnimationSequence()
        {
            return DOTween.Sequence()
            .Append(Target.DOLocalMove(EndValue, Duration, Snapping).SetEase(Ease).SetRelative(Relative));
        }
    }
}
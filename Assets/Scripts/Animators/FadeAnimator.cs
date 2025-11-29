using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Runtime.Utilities.Animators.FadableAdapters;

namespace Runtime.Utilities.Animators
{
    public class FadeAnimator : AbstractAnimator
    {
        [Tooltip("페이드 인 대상입니다.")]
        [SerializeField] private IFadableAdapter _fadable;
        [Tooltip("애니메이션 시작 시 대상값을 초기값으로 설정할지 여부를 결정합니다.")]
        public bool ApplyInitialValue = false;
        [ShowIf(nameof(ApplyInitialValue))]
        [Tooltip("애니메이션 시작 시 적용할 초기값입니다.")]
        public float InitialValue = 0f;
        [Tooltip("페이드 인 후 최종값입니다.")]
        public float EndValue = 1f;
        [Tooltip("애니메이션의 지속 시간(초)입니다.")]
        public float Duration = 0.5f;
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
                    _fadable.Value = InitialValue;
                }
            }
        }
        protected override void OnStart()
        {
            if (!ResetWithInitial)
            {
                if (ApplyInitialValue)
                {
                    _fadable.Value = InitialValue;
                }
            }
        }
        /// <summary>
        /// 대상에 페이드 인 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateAnimationSequence()
        {
            return DOTween.Sequence()
            .Append(DOTween.To(() => _fadable.Value, x => _fadable.Value = x, EndValue, Duration).SetEase(Ease));
        }
    }
}
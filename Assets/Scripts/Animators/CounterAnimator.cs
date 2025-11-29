using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Runtime.Utilities.Animators
{
    public class CounterAnimator : AbstractAnimator
    {
        [Tooltip("애니메이션 대상 오브젝트입니다.")]
        public TMP_Text Target;
        [Tooltip("카운트 시작값입니다.")]
        public int InitialValue = 0;
        [Tooltip("카운트 최종값입니다.")]
        public int EndValue = 100;
        [Tooltip("애니메이션의 지속 시간(초)입니다.")]
        public float Duration = 0.5f;
        [Tooltip("적용할 이징(Ease) 타입입니다.")]
        public Ease Ease = Ease.Linear;
        /// <summary>
        /// 카운터 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateAnimationSequence()
        {
            return DOTween.Sequence()
            .Append(Target.DOCounter(InitialValue, EndValue, Duration).SetEase(Ease));
        }
    }
}
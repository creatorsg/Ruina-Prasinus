using UnityEngine;
using DG.Tweening;

namespace Runtime.Utilities.Animators
{
    public class ShakeAnimator : AbstractAnimator
    {
        [Tooltip("애니메이션 대상 오브젝트입니다.")]
        public Transform Target;
        [Tooltip("애니메이션의 지속 시간(초)입니다.")]
        public float Duration = 0.1f;
        [Tooltip("진동 세기입니다.")]
        public float Strength = 0.1f;
        [Tooltip("초당 진동 횟수입니다.")]
        public int Vibrato = 50;
        [Tooltip("진동 방향의 무작위성 정도(0~180)입니다.")]
        public float Randomness = 90;
        [Tooltip("좌표 값을 정수로 강제할지 여부를 결정합니다.")]
        public bool Snapping = false;
        [Tooltip("활성화 시 진동이 점점 줄어들도록 처리합니다.")]
        public bool FadeOut = true;
        [Tooltip("진동 방향의 무작위성 모드를 설정합니다.")]
        public ShakeRandomnessMode ShakeRandomnessMode = ShakeRandomnessMode.Full;
        /// <summary>
        /// 진동 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateAnimationSequence()
        {
            return DOTween.Sequence()
            .Append(Target.DOShakePosition(Duration, Strength, Vibrato, Randomness, Snapping, FadeOut, ShakeRandomnessMode));
        }
    }
}
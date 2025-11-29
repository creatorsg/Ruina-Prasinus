using UnityEngine;
using DG.Tweening;
using Runtime.Utilities.Animators.Text.Character;

namespace Runtime.Utilities.Animators.Text
{
    public class TextParallelSequenceCreator : AbstractTextSequenceCreator
    {
        public override DOTweenTMPAnimator Animator
        {
            get => _animator;
            set
            {
                _animator = value;
                CharAnimator.Animator = value;
            }
        }
        [Tooltip("적용할 문자 애니메이터입니다.")]
        [field: SerializeField] public AbstractCharAnimator CharAnimator { get; private set; }
        [Tooltip("문자 간 애니메이션 간격입니다.")]
        [field: SerializeField] private float _characterInterval = 0.1f;
        [Tooltip("문자 간 애니메이션 최대 간격입니다.")]
        [field: SerializeField] private float _maxCharacterInterval = 0.3f;
        /// <summary>
        /// 텍스트 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateAnimationSequence()
        {
            Sequence sequence = DOTween.Sequence();
            float interval = 0;
            for (int index = 0; index < Animator.textInfo.characterCount; index++)
            {
                if (!Animator.textInfo.characterInfo[index].isVisible)
                {
                    continue;
                }
                Sequence charSequence = CharAnimator.CreateSequence(index);
                Sequence createSequence = DOTween.Sequence()
                .AppendInterval(interval)
                .Append(charSequence);

                sequence.Join(createSequence);

                interval += _characterInterval;
                if (interval >= _maxCharacterInterval)
                {
                    interval -= _maxCharacterInterval;
                }
            }
            return sequence;
        }
    }
}
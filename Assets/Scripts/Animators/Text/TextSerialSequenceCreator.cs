using UnityEngine;
using DG.Tweening;
using Runtime.Utilities.Animators.Text.Character;

namespace Runtime.Utilities.Animators.Text
{
    public class TextSerialSequenceCreator : AbstractTextSequenceCreator
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
        [Tooltip("초당 몇 개의 문자를 출력할지 설정합니다.")]
        [field: SerializeField] private float _charactersPerSecond = 30f;
        /// <summary>
        /// 텍스트 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected override Sequence CreateAnimationSequence()
        {
            Sequence sequence = DOTween.Sequence();
            for (int index = 0; index < Animator.textInfo.characterCount; index++)
            {
                if (!Animator.textInfo.characterInfo[index].isVisible)
                {
                    continue;
                }
                Sequence charSequence = CharAnimator.CreateSequence(index);
                Sequence createSequence = DOTween.Sequence()
                .AppendInterval(1 / _charactersPerSecond * index)
                .Append(charSequence);

                sequence.Join(createSequence);
            }
            return sequence;
        }
    }
}
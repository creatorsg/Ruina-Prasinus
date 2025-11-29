using DG.Tweening;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Utilities.Animators.Text.Character
{
    public class CompositeCharAnimator : AbstractCharAnimator
    {
        public override DOTweenTMPAnimator Animator
        {
            get => _animator;
            set
            {
                _animator = value;
                foreach (CharSequenceEntry entry in SequenceCreators)
                {
                    if (entry.Creator is AbstractCharAnimator abstractCharAnimator)
                    {
                        abstractCharAnimator.Animator = value;
                    }
                }
            }
        }
        [OdinSerialize] public List<CharSequenceEntry> SequenceCreators { get; private set; }
        public override bool IsInfiniteLoop
        {
            get
            {
                foreach (CharSequenceEntry entry in SequenceCreators)
                {
                    if (entry.Creator is AbstractCharAnimator abstractAnimator)
                    {
                        if (abstractAnimator.IsInfiniteLoop)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        protected override Sequence CreateCharAnimationSequence(int index)
        {
            if (Animator == null || index >= Animator.textInfo.characterCount)
            {
                Debug.Log("잘못된 DOTweenTMPAnimator입니다. index: " + index);
                return DOTween.Sequence();
            }

            Sequence sequence = DOTween.Sequence();
            foreach (CharSequenceEntry entry in SequenceCreators)
            {
                switch (entry.LinkType)
                {
                    case SequenceLinkType.Append:
                        sequence.Append(entry.Creator.CreateSequence(index));
                        break;
                    case SequenceLinkType.Join:
                        sequence.Join(entry.Creator.CreateSequence(index));
                        break;
                }
            }
            return sequence;
        }
    }
}
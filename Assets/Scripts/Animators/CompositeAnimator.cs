using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Serialization;
using Runtime.Utilities.Animators.Composite;

namespace Runtime.Utilities.Animators
{
    public class CompositeAnimator : AbstractAnimator
    {
        [OdinSerialize] public List<SequenceEntry> SequenceCreators { get; private set; }
        public override bool IsInfiniteLoop
        {
            get
            {
                foreach (SequenceEntry entry in SequenceCreators)
                {
                    if (entry.Creator is AbstractAnimator abstractAnimator)
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
        protected override Sequence CreateAnimationSequence()
        {
            Sequence sequence = DOTween.Sequence();
            foreach (SequenceEntry entry in SequenceCreators)
            {
                switch (entry.LinkType)
                {
                    case SequenceLinkType.Append:
                        sequence.Append(entry.Creator.CreateSequence());
                        break;
                    case SequenceLinkType.Join:
                        sequence.Join(entry.Creator.CreateSequence());
                        break;
                }
            }
            return sequence;
        }
    }
}
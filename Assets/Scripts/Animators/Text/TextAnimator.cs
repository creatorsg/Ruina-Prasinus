using DG.Tweening;
using System.Collections.Generic;
using Sirenix.Serialization;

namespace Runtime.Utilities.Animators.Text
{
    public class TextAnimator : AbstractTextAnimator
    {
        public override DOTweenTMPAnimator Animator
        {
            get => _animator;
            set
            {
                _animator = value;
                if (_printSequenceCreators != null)
                {
                    foreach (TextSequenceEntry entry in _printSequenceCreators)
                    {
                        entry.Creator.Animator = value;
                    }
                }
                if (_loopSequenceCreators != null)
                {
                    foreach (TextSequenceEntry entry in _loopSequenceCreators)
                    {
                        entry.Creator.Animator = value;
                    }
                }
            }
        }
        [OdinSerialize] private List<TextSequenceEntry> _printSequenceCreators;
        [OdinSerialize] private List<TextSequenceEntry> _loopSequenceCreators;
        protected override Sequence CreatePrintSequence()
        {
            Sequence sequence = DOTween.Sequence();
            if (_printSequenceCreators == null || _printSequenceCreators.Count == 0)
            {
                return sequence;
            }
            foreach (TextSequenceEntry entry in _printSequenceCreators)
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
        protected override Sequence CreateLoopSequence()
        {
            Sequence sequence = DOTween.Sequence();
            if (_loopSequenceCreators == null || _loopSequenceCreators.Count == 0)
            {
                return sequence;
            }
            foreach (TextSequenceEntry entry in _loopSequenceCreators)
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
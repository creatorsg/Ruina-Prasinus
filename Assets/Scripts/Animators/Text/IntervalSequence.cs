using UnityEngine;
using DG.Tweening;

namespace Runtime.Utilities.Animators.Text
{
    public class IntervalSequence : ITextSequenceCreator
    {
        public DOTweenTMPAnimator Animator { get; set; }
        [field: SerializeField] public float Interval { get; private set; }
        public Sequence CreateSequence()
        {
            return DOTween.Sequence().AppendInterval(Interval);
        }
    }
}
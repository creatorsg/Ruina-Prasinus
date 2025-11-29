using UnityEngine;
using DG.Tweening;

namespace Runtime.Utilities.Animators.Text.Character
{
    public class IntervalSequence : ICharSequenceCreator
    {
        [field:SerializeField] public float Interval { get; private set; }
        public Sequence CreateSequence(int index)
        {
            return DOTween.Sequence().AppendInterval(Interval);
        }
    }
}
using DG.Tweening;

namespace Runtime.Utilities.Animators.Text
{
    public interface ITextSequenceCreator
    {
        DOTweenTMPAnimator Animator { get; set; }
        Sequence CreateSequence();
    }
}
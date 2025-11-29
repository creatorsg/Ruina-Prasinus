using DG.Tweening;

namespace Runtime.Utilities.Animators.Text.Character
{
    public interface ICharSequenceCreator
    {
        Sequence CreateSequence(int index);
    }
}
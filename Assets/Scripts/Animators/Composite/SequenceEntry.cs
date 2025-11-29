using System;

namespace Runtime.Utilities.Animators.Composite
{
    [Serializable]
    public class SequenceEntry
    {
        public SequenceLinkType LinkType;
        public ISequenceCreator Creator;
    }
}
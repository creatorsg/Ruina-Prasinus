using System;
using UnityEngine;

namespace Runtime.Utilities.Animators.Text
{
    [Serializable]
    public class TextAnimatorEntry
    {
        [field:SerializeField] public SequenceLinkType LinkType { get; private set; }
        [field:SerializeField] public AbstractTextSequenceCreator SequenceCreator { get; private set; }
    }
}
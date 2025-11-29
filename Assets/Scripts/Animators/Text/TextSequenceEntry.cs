using System;
using UnityEngine;

namespace Runtime.Utilities.Animators.Text
{
    [Serializable]
    public class TextSequenceEntry
    {
        [field:SerializeField] public SequenceLinkType LinkType { get; private set; }
        [field:SerializeField] public ITextSequenceCreator Creator { get; private set; }
    }
}
using System;
using UnityEngine;

namespace Runtime.Utilities.Animators.Text.Character
{
    [Serializable]
    public class CharSequenceEntry
    {
        [field:SerializeField] public SequenceLinkType LinkType { get; private set; }
        [field:SerializeField] public ICharSequenceCreator Creator { get; private set; }
    }
}
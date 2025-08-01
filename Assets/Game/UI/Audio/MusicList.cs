using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicEntry
{
    public string displayName;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "Sounds", menuName = "Scriptable Objects/Sounds")]
public class MusicList : ScriptableObject
{
    public List<MusicEntry> entries;
}

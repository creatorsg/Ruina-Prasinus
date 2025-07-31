using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music", menuName = "Game/Audio/Music")]
public class Music : ScriptableObject
{
    public List<AudioClip> musics;
}

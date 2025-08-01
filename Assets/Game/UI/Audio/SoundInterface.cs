using Unity.VisualScripting;
using UnityEngine;

public class SoundInterface : Sound
{
    public static bool HasCached(string soundName) => Sounds.ContainsKey(soundName);
}

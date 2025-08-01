using Unity.VisualScripting;
using UnityEngine;

public class SoundLoad : Sound
{
    public static float GetSoundValue(string soundName, float defaultValue = 0.8f)
    {
        if (Sounds.TryGetValue(soundName, out var kc))
            return kc;

        float soundvalue = PlayerPrefs.GetFloat(Prefix + soundName, defaultValue);
        Sounds[soundName] = soundvalue;
        return soundvalue;
    }

    public static void ClearCache()
    {
        Sounds.Clear();
    }
}

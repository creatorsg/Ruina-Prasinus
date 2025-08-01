using UnityEngine;

public class SoundSave : Sound
{ 
    public static void saveSound(string soundName, float value, bool saveImmediately = true)
    {
        Sounds[soundName] = value;

        if (saveImmediately)
        {
            PlayerPrefs.SetFloat(Prefix + soundName, value);
            PlayerPrefs.Save();
        }
    }

    public static void SaveAll()
    {
        foreach (var kv in Sounds)
        {
            PlayerPrefs.SetFloat(Prefix + kv.Key, kv.Value);
        }
        PlayerPrefs.Save();
    }
}

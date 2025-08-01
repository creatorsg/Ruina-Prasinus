using System;
using UnityEngine;

public class SoundSave : Sound
{
    public event Action<string> OnSoundValueChanged;

    public static void SaveAll()
    {
        foreach (var kv in Sounds)
        {
            PlayerPrefs.SetFloat(Prefix + kv.Key, kv.Value);
        }
        PlayerPrefs.Save();
    }
}

using FMODUnity;
using UnityEngine;
using System.Collections.Generic;

public class AudioHandler : MonoBehaviour, IAudiosSetting
{
    private Dictionary<string, EventReference> _soundTrack = new Dictionary<string, EventReference>();
    public void PlaySound(string soundName)
    {
        
    }
    public void ChangeSound(string newSoundName)
    {

    }
    public void StopSound(string soundName)
    {

    }
    public void SetVolume(float volume)
    {

    }
}

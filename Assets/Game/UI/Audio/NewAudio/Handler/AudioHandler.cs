using FMODUnity;
using UnityEngine;
using System.Collections.Generic;

public class AudioHandler : MonoBehaviour, IAudiosSetting
{
    private string _currentSound;
    public void PlaySound(string soundName, GameObject _soundPlayer)
    {
        RuntimeManager.PlayOneShotAttached(soundName, _soundPlayer);
        _currentSound = soundName;
    }
    public void ChangeSound(string newSoundName)
    {
        PlaySound(newSoundName, this.gameObject);
    }
    public void StopSound(string soundName)
    {
        
    }
    public void SetVolume(float volume)
    {

    }
}

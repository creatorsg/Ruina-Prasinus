using UnityEngine;

public interface IAudiosSetting
{
    public void PlaySound(string soundName);
    public void ChangeSound(string newSoundName);
    public void StopSound(string soundName);
    public void SetVolume(float volume);
}   


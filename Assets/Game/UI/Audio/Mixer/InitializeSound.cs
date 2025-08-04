using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class InitializeSound
{
    public void initailize(AudioMixer mixer, string soundName, Slider slider)
    {
        if(string.IsNullOrWhiteSpace(soundName) || slider == null || mixer == null )
        {
            Debug.Log("현재 사운드 세팅에 설정값이 조정되지 않은 곳이 있습니다.");
            return;
        }

        float sound = 0f;

        bool hasParam = mixer.GetFloat(soundName, out sound);
        float dB = VolumeSave.Load(soundName, sound);

    }
}

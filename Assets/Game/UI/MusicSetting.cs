using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    [Header("Mixer & Audio")]
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public Slider volume;

    private void Start()
    {
        // 슬라이더 초기화 - 현재 믹서 값 읽어와서 반영
        if (audioMixer.GetFloat("MusicVolume", out float currentVolume))
        {
            volume.value = Mathf.Pow(10f, currentVolume / 20f); // dB → linear
        }

        volume.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        // linear → dB
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MusicVolume", dB);
    }
}
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public Slider BGM_Slider;

    float Master, BGM, SE, ME, BGS;
    private void Start()
    {
        InitialVolume("Master", out Master);
        InitialVolume("BGM", out BGM);
        InitialVolume("SE", out SE);
        InitialVolume("ME", out ME);
        InitialVolume("BGS", out BGS);


        BGM_Slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("BGM", dB);
    }

    private void InitialVolume(string volumeName, out float currentVolume)
    {
        if (audioMixer.GetFloat(volumeName, out currentVolume))
        {
            BGM_Slider.value = Mathf.Pow(10f, currentVolume / 20f);
        }
    }
}
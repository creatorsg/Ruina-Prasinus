using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Globalization;

public class MasterManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider Slider;
    public Text volume;

    private const float MinDb = -80f;
    private const float MaxDb = 20f;

    private void Start()
    {
        if (audioMixer.GetFloat("Master", out float currentVolume))
        {
            Slider.value = Mathf.InverseLerp(MinDb, MaxDb, currentVolume);
        }

        Slider.onValueChanged.AddListener(SetVolume);
        volume.text = (Slider.value * 100f).ToString("F2", CultureInfo.InvariantCulture) + "%";
    }

    private void SetVolume(float value)
    {
        float dB = Mathf.Lerp(MinDb, MaxDb, value);
        audioMixer.SetFloat("Master", dB);
        volume.text = (Slider.value * 100f).ToString("F2", CultureInfo.InvariantCulture) + "%";
    }
}
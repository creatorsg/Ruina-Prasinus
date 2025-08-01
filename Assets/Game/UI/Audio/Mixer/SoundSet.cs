using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[Serializable]
public class SoundSet
{
    public string parameterName;
    public AudioMixer audioMixer;
    public Slider slider;         
    public Text volumeText;

    private const float MinDb = -80f;
    private const float MaxDb = 20f;
    private UnityEngine.Events.UnityAction<float> onChangedCallback;

    public void Initialize()
    {
        if (audioMixer == null || slider == null || volumeText == null || string.IsNullOrWhiteSpace(parameterName))
        {
            Debug.LogWarning($"SoundSet Initialize skipped: invalid configuration for '{parameterName}'");
            return;
        }
        float mixerDb = 0f;
        bool hasParam = audioMixer.GetFloat(parameterName, out mixerDb);
        if (!hasParam)
        {
            Debug.LogWarning($"AudioMixer�� �Ķ���� '{parameterName}'��(��) �����ϴ�."); 
        }

        float dB = VolumeSave.Load(parameterName, mixerDb);

        slider.value = Mathf.InverseLerp(MinDb, MaxDb, dB);
        ApplyToMixerAndText(dB);

        onChangedCallback = OnSliderChanged;
        slider.onValueChanged.AddListener(onChangedCallback);
    }

    public void Cleanup()
    {
        if (slider != null && onChangedCallback != null)
            slider.onValueChanged.RemoveListener(onChangedCallback);
    }

    private void OnSliderChanged(float normalized)
    {
        float dB = Mathf.Lerp(MinDb, MaxDb, normalized);
        ApplyToMixerAndText(dB);
        VolumeSave.Save(parameterName, dB);
    }

    private void ApplyToMixerAndText(float dB)
    {
        audioMixer.SetFloat(parameterName, dB);
        volumeText.text = (slider.value * 100f).ToString("F2", CultureInfo.InvariantCulture) + "%";
    }

    public void LogStoredValue()
    {
        float storedDb = VolumeSave.Load(parameterName, 0f);
        Debug.Log($"[SoundSet] '{parameterName}'�� ����� dB: {storedDb}, �����̴� normalized: {slider?.value}");
    }
}

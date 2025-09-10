using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using System.Runtime.CompilerServices;

public class AudioSettingUI : MonoBehaviour
{
    [SerializeField] private Slider _seSlider;
    private FMOD.Studio.Bus _SE;

    private void Start()
    {
        _SE = RuntimeManager.GetBus("bus:/");

        _seSlider.onValueChanged.AddListener(v => _SE.setVolume(v));

        _SE.getVolume(out float bgmVolume);

        _seSlider.value = bgmVolume;
    }
}

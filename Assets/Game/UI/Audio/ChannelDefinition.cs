using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ChannelDefinition
{
    public string mixerParam;
    public AudioSource audioSource;
    public Text percentText;
    public Slider slider;
}

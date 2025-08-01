using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Sound
{
    public static Dictionary<string, float> Sounds = new();
    public const string Prefix = "Sounds_";

    public static event Action<string, float> OnSoundValueChanged;
}
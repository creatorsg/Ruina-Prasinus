using System.Collections.Generic;
using UnityEngine;

public class SoundGet : MonoBehaviour
{
    public List<SoundSet> channels = new();

    private void Start()
    {
        foreach (var ch in channels)
            ch.Initialize();
    }

    private void OnDestroy()
    {
        foreach (var ch in channels)
            ch.Cleanup();
    }

    private void LogAll()
    {
        foreach (var ch in channels)
            ch.LogStoredValue();
    }
}
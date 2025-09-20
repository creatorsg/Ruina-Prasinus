using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool _pause;

    public bool Pause => _pause;

    public void Start()
    {
        _pause = false;
    }
    public void StartPause()
    {
        _pause = true;
        Debug.Log("TRUE");
    }
}

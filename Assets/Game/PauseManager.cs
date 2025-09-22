using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool _pause;
    private MainPlayer _mainPlayer;
    public bool Pause => _pause;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
       _mainPlayer = player.GetComponent<MainPlayer>();
    }

    private void Start()
    {
        _pause = false;
    }
    public void StartPause()
    {
        _pause = true;
        _mainPlayer.InputHandler.enabled = false;
        Debug.Log("TRUE");
    }

    public void StartMonsterPause()
    {

    }
}

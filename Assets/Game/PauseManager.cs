using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool _pause;
    private MainPlayer _mainPlayer;

    // 이벤트: Pause 상태가 바뀔 때 알림
    public event Action<bool> OnPauseChanged;

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
        if (_pause) return; // 이미 pause면 중복 호출 방지

        _pause = true;
        _mainPlayer.InputHandler.enabled = false;

        // 이벤트 발행
        OnPauseChanged?.Invoke(_pause);
    }

    public void Resume()
    {
        if (!_pause) return;

        _pause = false;
        _mainPlayer.InputHandler.enabled = true;

        // 이벤트 발행
        OnPauseChanged?.Invoke(_pause);
    }
}

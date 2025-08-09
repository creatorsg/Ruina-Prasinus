using System;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private MainPlayer _player;

    private event Action<float> OnMove;
    private event Action OnDash;
    private event Action OnJump;

    private float _dashCooltime;
    private bool _dashRequested, _jumpRequested, _isDashHeld;
    private float _moveInput;

    public float MoveInput => _moveInput;
    public bool DashRequested => _dashRequested;
    public bool JumpRequested => _jumpRequested;
    public bool IsDashHeld => _isDashHeld;

    private void Awake()
    {
        OnMove += dir =>
        {
            _moveInput = dir;
        };

        OnDash += () =>
        {
            if (_dashCooltime > 0f)
            {
                _dashRequested = true;
            }
        };

        OnJump += () => _jumpRequested = true;
    }

    public void Initialize(MainPlayer player, float dashCooltime)
    {
        _player = player;
        _dashCooltime = dashCooltime;
    }

    private void Update()
    {
        MoveEvent();

        _isDashHeld = InputManager.GetKey("Dash");
    }

    public void MoveEvent()
    {
        float h = 0;

        if (InputManager.GetKey("MoveLeft"))
        {
            h += 1;
        }
        else if (InputManager.GetKey("MoveRight"))
        {
            h -= 1;
        }
        OnMove?.Invoke(h);

        if (InputManager.GetKeyDown("Dash"))
        {
            OnDash?.Invoke();
        }

        if (InputManager.GetKeyDown("Jump"))
        {
            OnJump?.Invoke();
        }
    }

    public void UseDashRequest()
    {
        _dashRequested = false;
    }
}

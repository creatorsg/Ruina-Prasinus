using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private MainPlayer _player;

    private event Action<float> OnMove;
    private event Action OnDash;
    private event Action OnJump;

    private float _dashCooltime, _dashcoolTimer = 0f;
    private bool _dashRequested, _jumpRequested, _isDashHeld, _canDash;
    private float _moveInput;

    public float MoveInput => _moveInput;
    public bool CanDash => _canDash;
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
            if (_canDash)
            {
                _dashRequested = true;
                _dashcoolTimer = 0f;
            }
        };

        OnJump += () =>
        {
            _jumpRequested = true;
            
        };
    }

    public void Initialize(MainPlayer player, float dashCooltime)
    {
        _player = player;
        _dashCooltime = dashCooltime;
    }

    private void Update()
    {
        MoveEvent();

        _isDashHeld = Input.GetKey(KeyCode.LeftShift);
        _dashcoolTimer = Mathf.Min(_dashcoolTimer + Time.deltaTime, _dashCooltime);

        _canDash = _dashcoolTimer >= _dashCooltime;
    }

    public void MoveEvent()
    {
        float h = 0;

        if (Input.GetKey(KeyCode.D))
        {
            h += 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            h -= 1;
        }
        OnMove?.Invoke(h);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDash?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump?.Invoke();
        }
    }

    public void UseDashRequest()
    {
        _dashRequested = false;
    }

    public void UseJumpRequest()
    {
        _jumpRequested = false;
    }
}

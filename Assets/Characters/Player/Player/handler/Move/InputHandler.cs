using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private MainPlayer _player;
    
    private event Action<float> OnMove;
    private event Action OnDash;
    private event Action OnJump;

    private float _dashCooltime, _dashcoolTimer = 0f;
    private bool _dashRequested, _jumpRequested, _isDashHeld, _canDash, _isJumpHeld;
    private float _jumpBufferTimer;
    private const float JUMP_BUFFER_TIME = 0.1f;
    private float _moveInput;

    public float MoveInput => _moveInput;
    public bool CanDash => _canDash;
    public bool DashRequested => _dashRequested;
    public bool JumpRequested => _jumpRequested;
    public bool IsDashHeld => _isDashHeld;
    public bool IsJumpHeld => _isJumpHeld;

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
            _jumpBufferTimer = 0f;
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

        _isDashHeld = InputManager.GetKey("Dash");
        _isJumpHeld = Input.GetKey(KeyCode.Space);
        _dashcoolTimer = Mathf.Min(_dashcoolTimer + Time.deltaTime, _dashCooltime);

        _canDash = _dashcoolTimer >= _dashCooltime;

        if (_jumpRequested)
        {
            _jumpBufferTimer += Time.deltaTime;
            if (_jumpBufferTimer > JUMP_BUFFER_TIME)
            {
                _jumpRequested = false;
            }
        }
    }

    public void MoveEvent()
    {
        float h = 0;

        if (InputManager.GetKey("MoveRight"))
        {
            h += 1;
        }
        else if (InputManager.GetKey("MoveLeft"))
        {
            h -= 1;
        }
        OnMove?.Invoke(h);

        if (InputManager.GetKeyDown("Dash"))
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
        _jumpBufferTimer = 0f; 
    }
}

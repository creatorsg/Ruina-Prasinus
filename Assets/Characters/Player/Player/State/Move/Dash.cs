using JetBrains.Annotations;
using UnityEngine;

public class Dash : State<MainPlayer>
{
    private float _maxDashSpeed;
    private float _dashAccelTime;
    private float _remainDashTime;
    private float _currentSpeed;
    private float _remainSpeed;

    private float _dashTimer;
    private float dt = Time.deltaTime;
    private Vector2 t;

    public Dash(float maxDashSpeed, float dashAccelTime, float remainDashTime, float maxWalkSpeed)
    {
        _maxDashSpeed = maxDashSpeed;
        _dashAccelTime = dashAccelTime;
        _remainDashTime = remainDashTime;
        _remainSpeed = maxWalkSpeed;
    }

    public override void Enter(MainPlayer player)
    {
        Debug.Log("Dash 진입");

        player.InputHandler.UseDashRequest();

        _dashTimer = 0f;
        _currentSpeed = 4.5f;
    }

    public override void Execute(MainPlayer player)
    { 
        if (player.InputHandler.IsDashHeld)
        {
            if (_dashTimer < _dashAccelTime)
            {
                float t = Mathf.Clamp01(_dashTimer / _dashAccelTime);
                _currentSpeed = Mathf.Lerp(0f, _maxDashSpeed, t);
            }
            else if (_dashTimer < _remainDashTime)
            {
                _currentSpeed = _maxDashSpeed;
            }
            else if (_dashTimer > _remainDashTime)
            {
                player.ChangeMoveState(MoveBehavior.Walk);
            }
        }
        else
        {
            player.ChangeMoveState(MoveBehavior.Walk);
        }

        t = new Vector2(player.MoveStatusHandler.Perp.x * _currentSpeed * player.WalkHandler.FacingDirection * dt,
                            player.MoveStatusHandler.Perp.y * _currentSpeed * player.WalkHandler.FacingDirection * dt);

        player.WalkHandler.WalkWhileDashing(_currentSpeed, _remainSpeed);
    }

    public override void FixedExecute(MainPlayer player)
    {
        _dashTimer += dt;

        if (player.MoveStatusHandler.IsSlope && player.MoveStatusHandler.IsGround)
            player.Rigidbody2D.linearVelocity = Vector2.zero;

        if (player.InputHandler.IsDashHeld)
        {
            player.transform.Translate(t, Space.World);
        }
    }

    public override void Exit(MainPlayer player)
    {
        _currentSpeed = 0f;
        _dashTimer = 0f;
    }
}

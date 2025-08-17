using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Walk : State<MainPlayer>
{
    private float _maxWalkSpeed, _walkAccelTime, _currentSpeed, _walkTimer;
    private float dt = Time.deltaTime;
    private Vector2 movePower;
    public Walk(float maxWalkSpeed, float walkAccelTime)
    {
        _maxWalkSpeed = maxWalkSpeed;
        _walkAccelTime = walkAccelTime;
    }
    public override void Enter(MainPlayer player)
    {
        if(player.MoveHandler.ReaminSpeed >= _maxWalkSpeed)
        {
            _currentSpeed = _maxWalkSpeed;
        }
        else
        {
            _currentSpeed = 0f;
            _walkTimer = 0f;
        }
        player.AnimatorManager?.SetMoveBool(true);
    }


    public override void Execute(MainPlayer player)
    {
        if (player.MoveHandler.IsWalking)
        {
            _walkTimer += dt;
            float t = Mathf.Clamp01(_walkTimer / _walkAccelTime);
            _currentSpeed = Mathf.Lerp(0f, _maxWalkSpeed, t);
        }
        else
        {
            _walkTimer = 0f;
            _currentSpeed = 0f;
            player.ChangeMoveState(MoveBehavior.Idle);
        }

        if (player.MoveHandler.IsWalking)
        {
            movePower = new Vector2(player.MoveStatusHandler.Perp.x * _currentSpeed * -player.InputHandler.MoveInput * dt,
                            player.MoveStatusHandler.Perp.y * _currentSpeed * -player.InputHandler.MoveInput * dt);
        }

        if (player.InputHandler.DashRequested && player.MoveStatusHandler.IsGround)
        {
            player.ChangeMoveState(MoveBehavior.Dash);
        }
    }

    public override void FixedExecute(MainPlayer player)
    {
        if (player.MoveHandler.IsWalking)
        {
            player.transform.Translate(movePower, Space.World);
        }

        if (player.InputHandler.JumpRequested && player.MoveStatusHandler.CanJump)
        {
            player.ChangeMoveState(MoveBehavior.Jump);
        }
    }

    public override void Exit(MainPlayer player)
    {
        if (!player.MoveHandler.IsWalking)
        {
            player.AnimatorManager?.SetMoveBool(false);
        }

        player.MoveHandler.RemainMoveSpeed(_currentSpeed);
        _currentSpeed = 0f;
        _walkTimer = 0f;
    }
}

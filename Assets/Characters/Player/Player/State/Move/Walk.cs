using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Walk : State<MainPlayer>
{
    private float _moveSpeed, dt = Time.deltaTime;
    private Vector2 movePower;
    public Walk()
    {
        
    }
    public override void Enter(MainPlayer player)
    {
        _moveSpeed = 5f; // 가속이 아닌 고정이기에 처음 들어올 때, 속도 조정 
    }

    public override void Execute(MainPlayer player)
    {
        if (player.MoveHandler.IsWalking)
        {
            {
                movePower = new Vector2(player.MoveStatusHandler.Perp.x * _moveSpeed * -player.InputHandler.MoveInput * dt,
                                player.MoveStatusHandler.Perp.y * _moveSpeed * -player.InputHandler.MoveInput * dt);
            }
        } 
        if (player.InputHandler.DashRequested && player.MoveStatusHandler.CanJump)
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
        player.MoveHandler.RemainMoveSpeed(_moveSpeed);
        _moveSpeed = 0f;
    }
}

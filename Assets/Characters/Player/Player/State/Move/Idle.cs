using UnityEngine;

public class Idle : State<MainPlayer>
{
    public override void Enter(MainPlayer player)
    {
        player.MoveHandler.RemainMoveSpeed(0f);
    }

    public override void Execute(MainPlayer player)
    {
        if (!player.PlayerHpHandler.IsHeating)
        {
            if (player.InputHandler.MoveInput != 0)
            {
                player.ChangeMoveState(MoveBehavior.Walk);
            }
            if (player.InputHandler.IsDashHeld && player.MoveStatusHandler.CanDash)
            {
                player.ChangeMoveState(MoveBehavior.Dash);
            }
        }

        if (player.YDeltaChecker.IsFalling && !player.PlayerHpHandler.IsHeating)
        {
            player.Rigidbody2D.AddForce(Vector2.down * 20f * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    public override void FixedExecute(MainPlayer player)
    {
        if (player.InputHandler.JumpRequested && player.MoveStatusHandler.CanJump)
        {
            player.ChangeMoveState(MoveBehavior.Jump);
        }
    }

    public override void Exit(MainPlayer player)
    {

    }
}

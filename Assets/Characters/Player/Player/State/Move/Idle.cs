using UnityEngine;

public class Idle : State<MainPlayer>
{
    private float _count;
    public override void Enter(MainPlayer player)
    {
        _count = 0;
    }

    public override void Execute(MainPlayer player)
    {
        if (!player.PlayerHpHandler.IsHeating)
        {
            if (player.InputHandler.MoveInput != 0)
            {
                player.ChangeMoveState(MoveBehavior.Walk);
            }
            if (player.InputHandler.DashRequested && player.MoveStatusHandler.CanDash)
            {
                player.ChangeMoveState(MoveBehavior.Dash);
            }
        }

        if (!player.PlayerHpHandler.IsHeating && _count != 130 && !player.MoveStatusHandler.CanJump)
        {
            player.Rigidbody2D.AddForce(Vector2.down * 20f * Time.deltaTime, ForceMode2D.Impulse);
            _count++;
        }
        else
        {
            _count = 0;
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

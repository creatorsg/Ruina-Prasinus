using UnityEngine;

public class Idle : State<MainPlayer>
{
    public override void Enter(MainPlayer player)
    {
        Debug.Log("Idle ¡¯¿‘");
        player.MoveHandler.RemainMoveSpeed(0f);
    }

    public override void Execute(MainPlayer player)
    {
        if(player.InputHandler.MoveInput != 0)
        {
            player.ChangeMoveState(MoveBehavior.Walk);
        }
        if(player.InputHandler.IsDashHeld)
        {
            player.ChangeMoveState(MoveBehavior.Dash);
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

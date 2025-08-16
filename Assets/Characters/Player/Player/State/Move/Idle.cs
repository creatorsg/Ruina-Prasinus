using UnityEngine;

public class Idle : State<MainPlayer>
{
    public override void Enter(MainPlayer player)
    {
        Debug.Log("Idle ¡¯¿‘");
        
    }

    public override void Execute(MainPlayer player)
    {
        if(player.InputHandler.MoveInput != 0)
        {
            player.ChangeMoveState(MoveBehavior.Walk);
        }
    }

    public override void FixedExecute(MainPlayer player)
    {
        if (player.InputHandler.JumpRequested && player.MoveStatusHandler.IsGround)
        {
            player.Rigidbody2D.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            player.InputHandler.UseJumpRequest();
        }
       if(player.MoveStatusHandler.IsJump && Input.GetKey(KeyCode.Space))
        {
            player.Rigidbody2D.AddForce(Vector2.up * 2f, ForceMode2D.Force);
        }
    }

    public override void Exit(MainPlayer player)
    {

    }
}

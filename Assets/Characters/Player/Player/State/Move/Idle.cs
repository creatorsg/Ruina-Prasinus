using UnityEngine;

public class Idle : State<MainPlayer>
{
    public override void Enter(MainPlayer player)
    {
        Debug.Log("Idle ¡¯¿‘");
        player.ChangeMoveState(MoveBehavior.Walk);
    }

    public override void Execute(MainPlayer player)
    {

    }

    public override void FixedExecute(MainPlayer player)
    {

    }

    public override void Exit(MainPlayer player)
    {

    }
}

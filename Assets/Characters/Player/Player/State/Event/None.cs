using UnityEngine;

public class None : State<MainPlayer>
{
    public override void Enter(MainPlayer player)
    {

    }

    public override void Execute(MainPlayer player)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            player.ChangeEventState(EventBehavior.Attack);
        }
    }
    public override void FixedExecute(MainPlayer player)
    {

    }
    public override void Exit(MainPlayer player)
    {

    }
}
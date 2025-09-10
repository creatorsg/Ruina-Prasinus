using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy3Detect : State<PurpleMushroom>
{
    private Vector2 _movePower;
    public override void Enter(PurpleMushroom enemy3)
    {
    }

    public override void Execute(PurpleMushroom enemy3)
    {
        if(enemy3.Enemy3StateHandler.IsMoving == true)
        {
            enemy3.ChangeState(Enemy3Behaviour.Walk);
        }
    }

    public override void FixedExecute(PurpleMushroom enemy3)
    {

    }

    public override void Exit(PurpleMushroom enemy3)
    {

    }
}

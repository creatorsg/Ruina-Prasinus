using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy3Detect : State<PurpleMushroom>
{
    public enum Phase { Walk, Stop };
    private Phase _currentPhase;
    private Vector2 _movePower;
    public override void Enter(PurpleMushroom enemy3)
    {
        _currentPhase = Phase.Walk;
    }

    public override void Execute(PurpleMushroom enemy3)
    {
        switch (_currentPhase)
        {
            case Phase.Walk:
                _movePower = new Vector2(enemy3.Enemy3DetectHandler.Perp.x * 5f * enemy3.Enemy3MoveHandler.MoveDirection * Time.deltaTime,
                                enemy3.Enemy3DetectHandler.Perp.y * 5f * enemy3.Enemy3MoveHandler.MoveDirection * Time.deltaTime);
                break;

            case Phase.Stop:
                _movePower = Vector2.zero;
                break;
        }

        if(enemy3.Enemy3DetectHandler.PlayerCheck)
        {

        }
    }

    public override void FixedExecute(PurpleMushroom enemy3)
    {
        switch (_currentPhase)
        {
            case Phase.Walk:
                enemy3.transform.Translate(_movePower, Space.World);
                break;

            case Phase.Stop:
                
                break;
        }
    }

    public override void Exit(PurpleMushroom enemy3)
    {

    }
}

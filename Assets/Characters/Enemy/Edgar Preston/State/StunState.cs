using UnityEngine;

public class StunState : State<Preston>
{
    public enum Phase { LieDown, StandUp }
    private Phase _currentPhase;
    private float _phaseTimer;
    public override void Enter(Preston boss)
    {
        _currentPhase = Phase.LieDown;
    }

    public override void Execute(Preston boss)
    {
        switch (_currentPhase)
        {
            case Phase.LieDown:
                _currentPhase = Phase.StandUp;
                break;

            case Phase.StandUp:
                boss.ChangeState(BossBehaviour.Idle);
                break;
        }
    }

    public override void FixedExecute(Preston boss)
    {
        switch (_currentPhase)
        {
            case Phase.LieDown:
                break;

            case Phase.StandUp:
                
                break;
        }
    }
    public override void Exit(Preston boss)
    {
        
    }
}

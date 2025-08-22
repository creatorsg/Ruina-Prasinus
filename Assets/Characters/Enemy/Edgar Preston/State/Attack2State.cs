using UnityEngine;

public class Attack2State : State<Preston>
{
    private enum Phase { AttackStand, Teleport, Attack }
    private Phase _currentPhase;
    private float _phaseTimer;
    private Vector2 _attackDirection;
    public override void Enter(Preston boss)
    {
        _currentPhase = Phase.AttackStand;
        _phaseTimer = 0f;
    }

    public override void Execute(Preston boss)
    {
       
    }

    public override void FixedExecute(Preston boss)
    {
       
    }
    public override void Exit(Preston boss)
    {
        
    }
}

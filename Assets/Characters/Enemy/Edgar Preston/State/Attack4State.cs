using UnityEngine;

public class Attack4State : State<Preston>
{
    public enum Phase { Charging, AttackStand, Rush }
    private Phase _currentPhase;
    private float _phaseTimer;
    public override void Enter(Preston boss)
    {
        Debug.Log(" Ω√¿€");
        _currentPhase = Phase.Charging;
    }

    public override void Execute(Preston boss)
    {
        boss.ChangeState(BossBehaviour.Stun);
    }

    public override void FixedExecute(Preston boss)
    {

    }
    public override void Exit(Preston boss)
    {
        
    }
}

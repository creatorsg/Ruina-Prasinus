using UnityEngine;

public class Attack4State : State<Preston>
{
    public enum Phase { Charging, AttackStand, Rush, Random }
    private Phase _currentPhase;
    private float _phaseTimer, _charging;
    public override void Enter(Preston boss)
    {
        Debug.Log(" ½ÃÀÛ");
        _charging = 0;
        _phaseTimer = 0;
        _currentPhase = Phase.Charging;
        Debug.Log("charging");
    }

    public override void Execute(Preston boss)
    {
        switch(_currentPhase)
        {
            case Phase.Charging:
                if (_phaseTimer == 0f)
                    _charging++;

                _phaseTimer += Time.deltaTime;

                if (_phaseTimer >= 2f)
                {
                    if (_charging != 3)
                        _currentPhase = Phase.Random;
                    else
                        _currentPhase = Phase.AttackStand;

                    _phaseTimer = 0f;
                }
                break;

            case Phase.Random:
                if(_charging == 1)
                {
                    _currentPhase = Phase.Charging;
                }

                if (_charging != 1)
                {
                    int a = Random.Range(0, 2);
                    if (a == 0)
                    {
                        _currentPhase = Phase.Charging;
                        boss.Pattern1Attack.ADDSize();
                        Debug.Log("charging");
                        _phaseTimer = 0f;
                    }
                    else if (a == 1)
                    {
                        _currentPhase = Phase.AttackStand;
                        _phaseTimer = 0f;
                    }
                }
                break;

            case Phase.AttackStand:
                _phaseTimer += Time.deltaTime;
                if (_phaseTimer >= 0.5)
                {
                    _currentPhase = Phase.Rush;
                    _phaseTimer = 0f;
                }
                break;

            case Phase.Rush:
                _phaseTimer += Time.deltaTime;
                boss.Rigidbody2D.linearVelocity = boss.DetectHandler.Dir * 20f;
                if (_phaseTimer > 0.5f)
                {
                    boss.ChangeState(BossBehaviour.Stun);
                }
                break;

        }
    }

    public override void FixedExecute(Preston boss)
    {

    }
    public override void Exit(Preston boss)
    {
        boss.Pattern1Attack.DeactivateAllPatterns();
    }
}

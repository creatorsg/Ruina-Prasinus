using UnityEngine;

public class Attack3State : State<Preston>
{
    public enum Phase { Moving, AttackStand, Crush }
    private Phase _currentPhase;
    private float _phaseTimer;
    public override void Enter(Preston boss)
    {
        Debug.Log("3패턴 시작");
        _currentPhase = Phase.Moving;
        _phaseTimer = 0f;
    }

    public override void Execute(Preston boss)
    {
        _phaseTimer += Time.deltaTime;
        switch (_currentPhase)
        {
            case Phase.Moving:
                if (_phaseTimer >= 2f)
                {
                    _phaseTimer = 0f;
                    _currentPhase = Phase.AttackStand;
                    Debug.Log("3패턴: 공격 준비 자세");
                }
                break;

            case Phase.AttackStand:
                if (_phaseTimer >= 1f)
                {
                    _phaseTimer = 0f;
                    _currentPhase = Phase.Crush;
                    Debug.Log("3패턴: 내려찍기!");
                }
                break;

            case Phase.Crush:
                if (_phaseTimer >= 0.5f)
                {
                    Debug.Log("3패턴 종료");
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
        Debug.Log("3패턴 탈출");
    }
}

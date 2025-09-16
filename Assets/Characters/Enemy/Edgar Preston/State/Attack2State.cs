using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Attack2State : State<Preston>
{
    private enum Phase { AttackStand, Teleport, Attack }
    private Phase _currentPhase;
    private float _phaseTimer, _teleportIndex;
    private Vector2 _start, _end;
    private Vector2 _attackDirection;
    RaycastHit2D _hit;
    public override void Enter(Preston boss)
    {
        Debug.Log("패턴2 시작");
        _currentPhase = Phase.AttackStand;
        _phaseTimer = 0f;
        _teleportIndex = 0;
    }

    public override void Execute(Preston boss)
    {
        _phaseTimer += Time.deltaTime;

        switch (_currentPhase)
        {
            case Phase.AttackStand:
                if (_phaseTimer >= 0.1f)
                {
                    _phaseTimer = 0f;
                    _currentPhase = Phase.Teleport;
                }
                break;

            case Phase.Teleport:
                if(_phaseTimer >= 0.5)
                {
                    _phaseTimer = 0f;
                    _currentPhase = Phase.Attack;

                    _start = boss.transform.position;
                    _end = new Vector2(_start.x, -10);
                }
                break;

            case Phase.Attack:
                if (_phaseTimer >= 0.5f)
                {
                    Debug.Log("패턴 2 종료");
                    boss.ChangeState(BossBehaviour.Stun);
                }
                break;
        }
    }

    public override void FixedExecute(Preston boss)
    {
        switch (_currentPhase)
        {
            case Phase.AttackStand:
                break;

            case Phase.Teleport:
                if (_teleportIndex == 0)
                {
                    boss.transform.position = new Vector2(boss.DetectHandler.Player.transform.position.x, 2);
                    _teleportIndex++;
                }
                else
                {
                    boss.Rigidbody2D.linearVelocity = Vector2.zero;
                }
                break;

            case Phase.Attack:
                boss.Rigidbody2D.AddForce(Vector2.down * 20f, ForceMode2D.Force);
                break;


                
        }
    }
    public override void Exit(Preston boss)
    {
      
    }
}

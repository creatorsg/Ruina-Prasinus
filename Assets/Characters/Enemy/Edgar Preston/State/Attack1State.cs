using UnityEngine;

public class Attack1State : State<Preston>
{
    private enum Phase { Walking, Pausing, Dashing }
    private Phase _currentPhase;
    private float _phaseTimer;
    private Vector2 _attackDirection;
    public override void Enter(Preston boss)
    {
        _phaseTimer = 0f;
        _currentPhase = Phase.Walking;

        float directionX = Mathf.Sign(boss.transform.localScale.x);
        _attackDirection = new Vector2(directionX, 0);
    }

    public override void Execute(Preston boss)
    {
        _phaseTimer += Time.deltaTime;

        switch (_currentPhase)
        {
            case Phase.Walking:
                if (_phaseTimer >= 3f)
                {
                    _phaseTimer = 0f;
                    _currentPhase = Phase.Pausing;
                }
                break;

            case Phase.Pausing:
                if (_phaseTimer >= 0.5f)
                {
                    _phaseTimer = 0f; 
                    _currentPhase = Phase.Dashing;

                    //boss.Pattern1Attack.DashAttack();
                }
                break;

            case Phase.Dashing:
                if (_phaseTimer >= 0.5f)
                {
                    Debug.Log("패턴 1 종료");
                    boss.ChangeState(BossBehaviour.Stun);
                }
                break;
        }
    }

    public override void FixedExecute(Preston boss)
    {
        switch (_currentPhase)
        {
            case Phase.Walking:
                boss.Rigidbody2D.linearVelocity = _attackDirection * 2f;
                break;

            case Phase.Pausing:
                boss.Rigidbody2D.linearVelocity = Vector2.zero;
                break;

            case Phase.Dashing:
                boss.Rigidbody2D.linearVelocity = _attackDirection * 10f;
                break;
        }
    }
    public override void Exit(Preston boss)
    {
        if (boss.Rigidbody2D != null)
        {
            boss.Rigidbody2D.linearVelocity = Vector2.zero;
        }
    }
}

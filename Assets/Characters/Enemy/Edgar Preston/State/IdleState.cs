using UnityEngine;

public class IdleState : State<Preston>
{
    private float _timer;
    private BossBehaviour _nextPattern;
    public override void Enter(Preston boss)
    {
        Debug.Log("Idle Ω√¿€");
        _timer = 0f;

        int pattern = boss.Pattern1Attack.RandomPattern();
        _nextPattern = (BossBehaviour)pattern;
    }

    public override void Execute(Preston boss)
    {
        _timer += Time.deltaTime;

        if (_timer >= 0.5f)
        {
            boss.ChangeState(_nextPattern);
        }
    }

    public override void FixedExecute(Preston boss)
    {

    }
    public override void Exit(Preston boss)
    {
        Debug.Log("Idle ≈ª√‚");
    }
}

using UnityEngine;

public class IdleState : State<Preston>
{
    private float _timer;
    private BossBehaviour _nextPattern;
    public override void Enter(Preston boss)
    {
        _timer = 0f;
        boss.DetectHandler.DetectPlayer();
        int pattern = boss.Pattern1Attack.RandomPattern();
        boss.Pattern1Attack.ActivatePattern(pattern);
        _nextPattern = (BossBehaviour)pattern;
    }

    public override void Execute(Preston boss)
    {
        Debug.Log(boss.DetectHandler.IsGround);

        if (boss.DetectHandler.IsGround)
        {
            _timer += Time.deltaTime;

            if (_timer >= 0.5f)
            {
                boss.ChangeState(_nextPattern);
            }
        }
    }

    public override void FixedExecute(Preston boss)
    {

    }
    public override void Exit(Preston boss)
    {
    }
}

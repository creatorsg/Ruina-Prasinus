using UnityEngine;

public class Enemy3Rush : State<PurpleMushroom>
{
    public enum Phase { surprised, rush }
    private Phase _currentPhase;
    private float _direction, _rushSpeed, _surpriseTimer;
    private Vector2 _movePower;

    public Enemy3Rush(float rushSpeed)
    {
        _rushSpeed = rushSpeed;
    }
    public override void Enter(PurpleMushroom enemy3)
    {
        _direction = -enemy3.Enemy3StateHandler.Dir;
        _movePower = Vector2.zero;
    }

    public override void Execute(PurpleMushroom enemy3)
    {
        switch(_currentPhase)
        {
            case Phase.surprised:
                _surpriseTimer += Time.deltaTime;
                if(_surpriseTimer >= 0.5f)
                {
                    _currentPhase = Phase.rush;
                }
                break;

            case Phase.rush:
                _movePower = new Vector2(enemy3.Enemy3StateHandler.Perp.x * _rushSpeed * _direction * Time.fixedDeltaTime,
                                         enemy3.Enemy3StateHandler.Perp.y * _rushSpeed * _direction * Time.fixedDeltaTime);
                break;
        }        
    }

    public override void FixedExecute(PurpleMushroom enemy3)
    {
        enemy3.transform.Translate(_movePower, Space.World);
    }

    public override void Exit(PurpleMushroom enemy3)
    {
        
    }
}

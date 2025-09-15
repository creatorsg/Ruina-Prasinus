using UnityEngine;
using UnityEngine.Rendering;

public class Enemy3Walk : State<PurpleMushroom>
{
    public enum Phase { walk, stop};
    private Phase _currentPhase;
    private float _walkSpeed, _walkTimer, _stopTimer;
    private Vector2 _movePower;

    public Enemy3Walk(float walkSpeed)
    {
        _walkSpeed = walkSpeed;
    }

    public override void Enter(PurpleMushroom enemy3)
    {
        _currentPhase = Phase.walk;
    }

    public override void Execute(PurpleMushroom enemy3)
    {
        switch (_currentPhase)
        {
            case Phase.walk:
                _movePower = new Vector2(enemy3.Enemy3StateHandler.Perp.x * _walkSpeed * -enemy3.Enemy3StateHandler.Dir * Time.fixedDeltaTime,
                                         enemy3.Enemy3StateHandler.Perp.y * _walkSpeed * -enemy3.Enemy3StateHandler.Dir * Time.fixedDeltaTime);
                _walkTimer += Time.deltaTime;
                if (_walkTimer >= 3f)
                {
                    _movePower = Vector2.zero;  
                    _currentPhase = Phase.stop;
                    _walkTimer = 0;
                }
                break;

            case Phase.stop:
                _movePower = Vector2.zero;
                _stopTimer += Time.deltaTime;
                if (_stopTimer >= 0.5f)
                {
                    enemy3.Enemy3MoveHandler.ChangeDirection();
                    _currentPhase = Phase.walk;
                    _stopTimer = 0;
                }
                break;
        }

        if(enemy3.Enemy3StateHandler.PlayerCheck)
        {
            enemy3.ChangeState(Enemy3Behaviour.Rush);
        }
    }


    public override void FixedExecute(PurpleMushroom enemy3)
    {
        switch (_currentPhase)
        {
            case Phase.walk:
                enemy3.transform.Translate(_movePower, Space.World);
                break;

            case Phase.stop:

                break;
        }
    }

    public override void Exit(PurpleMushroom enemy3)
    {

    }
}

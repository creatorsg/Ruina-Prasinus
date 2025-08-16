using UnityEngine;

public class Enemy3Idle : State<PurpleMushrooms>
{
    private float _moveSpeed, _detectionDistance, _moveTimer;
    public Enemy3Idle(float moveSpeed,float detectionDistance)
    {
        _moveSpeed = moveSpeed;
        _detectionDistance = detectionDistance;
    }
    public override void Enter(PurpleMushrooms enemy3)
    {
        Debug.Log("진입");
        _moveTimer = 0f;
    }

    public override void Execute(PurpleMushrooms enemy3)
    {
        enemy3.MoveHandler.Detection(_detectionDistance);
        enemy3.GroundCheck.GroundCheck();

        if(_moveTimer >= 7f)
        {
            enemy3.ChangeState(Enemy3Behaviour.Stop);
            _moveTimer = 0f;
        }
    }

    public override void FixedExecute(PurpleMushrooms enemy3)
    {
        if (enemy3.GroundCheck.IsGround)
        {
            float moveDirection = enemy3.MoveHandler.MoveDirection;

            enemy3.Rigidbody2D.linearVelocity = new Vector2(moveDirection * _moveSpeed, 0);
            _moveTimer += Time.fixedDeltaTime;
        }
    }

    public override void Exit(PurpleMushrooms enemy3)
    {
        enemy3.Rigidbody2D.linearVelocity = Vector3.zero;
        _moveTimer = 0f;
    }
}

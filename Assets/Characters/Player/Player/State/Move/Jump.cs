using UnityEngine;

public class Jump : State<MainPlayer>
{
    private float _jumpPower, _jumpAccelPower, _jumpRemainTime;
    private float _moveSpeed, _moveDirection;
    private float _currentSpeed, _walkTimer;
    private float _walkAccelTime, _maxWalkSpeed, dt = Time.deltaTime;

    public Jump(float jumpPower, float jumpAccelPower, float jumpRemainTime)
    {
        _jumpPower = jumpPower;
        _jumpAccelPower = jumpAccelPower;
        _jumpRemainTime = jumpRemainTime;
    }

    public override void Enter(MainPlayer player)
    {
        _currentSpeed = 5f;
        _moveSpeed = player.MoveHandler.ReaminSpeed;
        player.Rigidbody2D.linearVelocity = new Vector2(0, 0);
        player.Rigidbody2D.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse);
    }

    public override void Execute(MainPlayer player)
    {
        if (player.MoveHandler.IsWalking )
        {
            _currentSpeed = 5f;

            if (player.MoveStatusHandler.CanJump)
            {
                player.ChangeMoveState(MoveBehavior.Walk);
            }
        }
        else
        {
            _walkTimer = 0f;
            _currentSpeed = 0f;
        }

        if (player.MoveStatusHandler.CanJump && !player.MoveHandler.IsWalking)
        {
            player.ChangeMoveState(MoveBehavior.Idle);
        }
    }

    public override void FixedExecute(MainPlayer player)
    {
        if(player.InputHandler.IsJumpHeld)
        {
            player.Rigidbody2D.AddForce(Vector2.up * 2f, ForceMode2D.Force);
        }
        player.Rigidbody2D.linearVelocity = new Vector2(_currentSpeed * player.MoveHandler.MoveDirection, player.Rigidbody2D.linearVelocityY);
    }

    public override void Exit(MainPlayer player)
    {   
        Debug.Log("점프 종료");
        player.InputHandler.UseJumpRequest();
    }
}

using UnityEngine;

public class Jump : State<MainPlayer>
{
    private float _jumpPower, _jumpAccelPower, _jumpRemainTime;
    private float _moveSpeed, _moveDirection;
    private float _currentSpeed, _walkTimer;
    private float _walkAccelTime, _maxWalkSpeed, dt = Time.deltaTime;

    public Jump(float jumpPower, float jumpAccelPower, float jumpRemainTime, float walkAccelTime, float maxWalkSpeed)
    {
        _jumpPower = jumpPower;
        _jumpAccelPower = jumpAccelPower;
        _jumpRemainTime = jumpRemainTime;
        _walkAccelTime = walkAccelTime;
        _maxWalkSpeed = maxWalkSpeed;
    }

    public override void Enter(MainPlayer player)
    {
        Debug.Log("점프 진입");
        _moveSpeed = player.MoveHandler.ReaminSpeed;
        player.Rigidbody2D.linearVelocity = new Vector2(0, 0);
        player.Rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
    }

    public override void Execute(MainPlayer player)
    {
        if (player.MoveHandler.IsWalking)
        {
            _walkTimer += dt;
            float t = Mathf.Clamp01(_walkTimer / _walkAccelTime);
            _currentSpeed = Mathf.Lerp(_moveSpeed, _maxWalkSpeed, t);

            if (player.MoveStatusHandler.CanJump && player.MoveStatusHandler.IsGround)
            {
                player.ChangeMoveState(MoveBehavior.Walk);
            }
        }
        else
        {
            _walkTimer = 0f;
            _currentSpeed = 0f;
        }

        if (player.MoveStatusHandler.CanJump && !player.MoveHandler.IsWalking && player.MoveStatusHandler.IsGround)
        {
            player.ChangeMoveState(MoveBehavior.Idle);
        }
    }

    public override void FixedExecute(MainPlayer player)
    {
        if(player.InputHandler.IsJumpHeld)
        {
            player.Rigidbody2D.AddForce(Vector2.up * _jumpAccelPower, ForceMode2D.Force);
        }
        player.Rigidbody2D.linearVelocity = new Vector2(_currentSpeed * player.MoveHandler.MoveDirection, player.Rigidbody2D.linearVelocity.y);
    }

    public override void Exit(MainPlayer player)
    {
        Debug.Log("점프 종료");
        player.InputHandler.UseJumpRequest();
    }
}

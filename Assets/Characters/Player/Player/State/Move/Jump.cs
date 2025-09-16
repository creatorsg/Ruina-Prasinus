using UnityEngine;

public class Jump : State<MainPlayer>
{
    private float _jumpPower, _jumpAccelPower, _jumpRemainTime, _maxJumpTime;
    private float _moveSpeed, _moveDirection;
    private float _currentSpeed, _walkTimer, _jumpTimer;
    private float _walkAccelTime, _maxWalkSpeed, dt = Time.deltaTime;
    private bool _isFalling;
    private Rigidbody2D rb;
    public Jump(float jumpPower, float jumpAccelPower, float jumpRemainTime)
    {
        _jumpPower = jumpPower;
        _jumpAccelPower = jumpAccelPower;
        _jumpRemainTime = jumpRemainTime;
    }

    public override void Enter(MainPlayer player)
    {
        rb = player.Rigidbody2D;
        _isFalling = false;
        _currentSpeed = 5f;
        _jumpTimer = 0f;
        _maxJumpTime = 0.1f;
        player.Rigidbody2D.linearVelocity = new Vector2(0, 0);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 5f);
    }

    public override void Execute(MainPlayer player)
    {
        if (player.InputHandler.IsJumpHeld && _jumpTimer <= _maxJumpTime)
        {
            rb.AddForce(Vector2.up * 50f * Time.deltaTime, ForceMode2D.Impulse);
            _jumpTimer += Time.deltaTime;
        }

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

        if(_jumpTimer > _maxJumpTime)
        {
            _isFalling = true;
        }

        if(player.YDeltaChecker.IsFalling)
        {
            rb.AddForce(Vector2.down * 20f * Time.deltaTime, ForceMode2D.Impulse);
        }
        
    }

    public override void FixedExecute(MainPlayer player)
    {
        player.Rigidbody2D.linearVelocity = new Vector2(_currentSpeed * player.MoveHandler.MoveDirection, player.Rigidbody2D.linearVelocityY);
    }

    public override void Exit(MainPlayer player)
    {   
        Debug.Log("점프 종료");
        player.InputHandler.UseJumpRequest();
    }
}

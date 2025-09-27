using UnityEngine;

public class Jump : State<MainPlayer>
{
    private float _jumpPower, _jumpAccelPower, _jumpRemainTime, _maxJumpTime;
    private float _moveSpeed, _moveDirection;
    private float _currentSpeed, _walkTimer, _jumpTimer;
    private float _walkAccelTime, _maxWalkSpeed, dt = Time.deltaTime, _count;
    private bool _isFalling;
    public Jump(float jumpPower, float jumpAccelPower, float jumpRemainTime)
    {
        _jumpPower = jumpPower;
        _jumpAccelPower = jumpAccelPower;
        _jumpRemainTime = jumpRemainTime;
    }

    public override void Enter(MainPlayer player)
    {
        player.footsound.PlayJumpSound();

        if(player.MoveHandler.IsDashJump)
        {
            _currentSpeed = 10f;
        }
        else
        {
            _currentSpeed = 5f;
        }

        _maxJumpTime = 0.13f;
        _jumpTimer = 0f;
        _count = 0;

        player.Rigidbody2D.linearVelocity = Vector2.zero;
        player.Rigidbody2D.linearVelocity = new Vector2(player.Rigidbody2D.linearVelocity.x, 5f);
    }

    public override void Execute(MainPlayer player)
    {
        if (player.InputHandler.IsJumpHeld && _jumpTimer <= _maxJumpTime)
        {
            player.Rigidbody2D.AddForce(Vector2.up * 50f * Time.deltaTime, ForceMode2D.Impulse);
            _jumpTimer += Time.deltaTime;
        }

        if (player.MoveHandler.IsWalking)
        {
            if (!player.MoveHandler.IsDashJump)
            {
                _currentSpeed = 5f;
            }
            if (player.MoveStatusHandler.CanJump && !player.MoveHandler.IsDashJump)
            {
                Debug.Log("Walk로 이동");
                player.ChangeMoveState(MoveBehavior.Walk);
            }
        }
        else
        {
            _walkTimer = 0f;
            _currentSpeed = 0f;
        }

        if(!player.MoveHandler.IsWalking)
        {
            player.MoveHandler.EndDash();
        }

        if (player.MoveStatusHandler.CanJump && !player.MoveHandler.IsWalking && !player.InputHandler.IsJumpHeld)
        {
            Debug.Log("idle로 이동");
            player.ChangeMoveState(MoveBehavior.Idle);
        }

        if(_jumpTimer > _maxJumpTime || !player.InputHandler.IsJumpHeld)
        {
            _isFalling = true;
            if(player.MoveStatusHandler.CanJump && player.MoveHandler.IsWalking)
            {
                player.ChangeMoveState(MoveBehavior.Walk);
            }
        }

        if (_isFalling == true && !player.PlayerHpHandler.IsHeating && _count != 130)
        {
            player.Rigidbody2D.AddForce(Vector2.down * 20f * Time.deltaTime, ForceMode2D.Impulse);
            _count++;
        }
        else
        {
            _count = 0;
        }
    }

    public override void FixedExecute(MainPlayer player)
    {
        player.Rigidbody2D.linearVelocity = new Vector2(_currentSpeed * player.MoveHandler.MoveDirection, player.Rigidbody2D.linearVelocityY);
    }

    public override void Exit(MainPlayer player)
    {
        player.MoveHandler.EndDash();
        player.InputHandler.UseJumpRequest();
        Debug.Log("점프 종료");
        player.Rigidbody2D.linearVelocity = Vector2.zero;
        player.Rigidbody2D.angularVelocity = 0f;
    }
}

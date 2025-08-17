using UnityEngine;

public class Jump : State<MainPlayer>
{
    private float _jumpPower, _jumpAccelPower, _jumpRemainTime;
    private float _moveSpeed, _moveDirection;

    public Jump(float jumpPower, float jumpAccelPower, float jumpRemainTime)
    {
        _jumpPower = jumpPower;
        _jumpAccelPower = jumpAccelPower;
        _jumpRemainTime = jumpRemainTime;
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
        if (player.MoveStatusHandler.CanJump)
        {
            player.ChangeMoveState(MoveBehavior.Walk);
        }
    }

    public override void FixedExecute(MainPlayer player)
    {
        if(player.InputHandler.IsJumpHeld)
        {
            player.Rigidbody2D.AddForce(Vector2.up * _jumpAccelPower, ForceMode2D.Force);
        }
        _moveSpeed = Mathf.Lerp(_moveSpeed, 0f, 0.1f * Time.fixedDeltaTime);
        player.Rigidbody2D.linearVelocity = new Vector2(_moveSpeed * player.MoveHandler.MoveDirection, player.Rigidbody2D.linearVelocity.y);
    }

    public override void Exit(MainPlayer player)
    {
        Debug.Log("점프 종료");
        player.InputHandler.UseJumpRequest();
    }
}

using UnityEngine;

public class Dash : State<MainPlayer>
{
    private float _maxDashSpeed, _dashAccelTIme, _dashRemainTime, _dashTimer, _currentDashSpeed, _dashCooltime;
    private float dt = Time.deltaTime;
    private Vector2 t;
    public Dash(float maxDashSpeed, float dashAccelTime, float dashRemainTime)
    {
        _maxDashSpeed = maxDashSpeed;
        _dashAccelTIme = dashAccelTime;
        _dashRemainTime = dashRemainTime;
    }

    public override void Enter(MainPlayer player)
    {
        Debug.Log("대쉬 진입");

        player.InputHandler.UseDashRequest();

        _dashTimer = 0f;
        _currentDashSpeed = 0f;
    }

    public override void Execute(MainPlayer player)
    {
        if (player.InputHandler.IsDashHeld && player.MoveHandler.IsWalking)
        { 
            if (_dashTimer <= _dashAccelTIme)
            {
                float t = Mathf.Clamp01(_dashTimer / _dashAccelTIme);
                _currentDashSpeed = Mathf.Lerp(0f, _maxDashSpeed, t);
            }
            else if (_dashTimer < _dashRemainTime)
            {
                _currentDashSpeed = _maxDashSpeed;
            }
            else if (_dashTimer > _dashRemainTime && player.MoveHandler.IsWalking)
                player.ChangeMoveState(MoveBehavior.Walk);
            else
                player.ChangeMoveState(MoveBehavior.Idle);
        }
        else if (!player.InputHandler.IsDashHeld && player.MoveHandler.IsWalking)
        {
            player.ChangeMoveState(MoveBehavior.Walk);
        }
        else
            player.ChangeMoveState(MoveBehavior.Idle);

        t = new Vector2(player.MoveStatusHandler.Perp.x * _currentDashSpeed * -player.MoveHandler.MoveDirection * dt,
                            player.MoveStatusHandler.Perp.y * _currentDashSpeed * -player.MoveHandler.MoveDirection * dt);
        
        if (!player.MoveStatusHandler.IsGround)
        {
            if (t.y != 0)
            {
                t.y = 0;
            }
        }

    }

    public override void FixedExecute(MainPlayer player)
    {
        _dashTimer += Time.deltaTime;

        if (player.MoveHandler.IsWalking)
        {
            player.transform.Translate(t, Space.World);
        }

        if (player.InputHandler.JumpRequested && player.MoveStatusHandler.IsGround && _dashTimer <= _dashAccelTIme)
        {
            if (t.y != 0)
            {
                t.y = 0;
            }
            player.Rigidbody2D.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            player.InputHandler.UseJumpRequest();
        }
        if (player.MoveStatusHandler.IsJump && Input.GetKey(KeyCode.Space))
        {
            player.Rigidbody2D.AddForce(Vector2.up * 2f, ForceMode2D.Force);
        }
    }

    public override void Exit(MainPlayer player)
    {
        Debug.Log("대쉬 종료");
    }
}

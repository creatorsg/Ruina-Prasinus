using UnityEngine;

public class Dash : State<MainPlayer>
{
    private float _maxDashSpeed, _dashAccelTIme, _dashRemainTime = 0.5f, _dashTimer, _currentDashSpeed, _dashCooltime;
    private float dt = Time.deltaTime;
    private Vector2 movePower;
    public Dash()
    {
        
    }

    public override void Enter(MainPlayer player)
    {
        Debug.Log("대쉬 진입");

        _dashTimer = 0f;
        _currentDashSpeed = 10f;
    }

    public override void Execute(MainPlayer player)
    {
        if (player.InputHandler.IsDashHeld && player.MoveHandler.IsWalking)
        {
            if (_dashTimer < _dashRemainTime)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    movePower = Vector2.zero;
                    player.ChangeMoveState(MoveBehavior.Jump);
                }
            }
            else if (_dashTimer > _dashRemainTime && player.MoveHandler.IsWalking)
            {
                player.ChangeMoveState(MoveBehavior.Walk);
                Debug.Log("walk로 이동");
            }
            else if(_dashTimer > _dashRemainTime && !player.MoveHandler.IsWalking)
                player.ChangeMoveState(MoveBehavior.Idle);
        }
        else if (!player.InputHandler.IsDashHeld && player.MoveHandler.IsWalking)
        {
            player.ChangeMoveState(MoveBehavior.Walk);
        }
        else
            player.ChangeMoveState(MoveBehavior.Idle);

        if (!player.MoveStatusHandler.IsGround)
        {
            if (movePower.y != 0)
            {
                movePower.y = 0;
            }
        }

    }

    public override void FixedExecute(MainPlayer player)
    {
        movePower = new Vector2(player.MoveStatusHandler.Perp.x * _currentDashSpeed * -player.InputHandler.MoveInput * dt,
                                 player.MoveStatusHandler.Perp.y * _currentDashSpeed * -player.InputHandler.MoveInput * dt);

        player.transform.Translate(movePower, Space.World);
    }

    public override void Exit(MainPlayer player)
    {
        player.InputHandler.UseDashRequest();
        Debug.Log("대쉬 종료");
    }
}

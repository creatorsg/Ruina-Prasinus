using Player;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Walk : State<MainPlayer>
{
    private GameObject moving;
    private float _maxWalkSpeed, _walkAccelTime, _currentSpeed, _walkTimer;
    private float dt = Time.deltaTime;
    private Vector2 t;
    public Walk(float maxWalkSpeed, float walkAccelTime)
    {
        _maxWalkSpeed = maxWalkSpeed;
        _walkAccelTime = walkAccelTime;
    }
    public override void Enter(MainPlayer player)
    {
        player.AnimatorManager?.SetMoveBool(true);
        _currentSpeed = 0f;
        _walkTimer = 0f;
    }


    public override void Execute(MainPlayer player)
    {
        if (player.MoveHandler.IsWalking)
        {
            _walkTimer += dt;
            float t = Mathf.Clamp01(_walkTimer / _walkAccelTime);
            _currentSpeed = Mathf.Lerp(0f, _maxWalkSpeed, t);
        }
        else
        {
            _walkTimer = 0f;
            _currentSpeed = 0f;
            player.ChangeMoveState(MoveBehavior.Idle);
        }

        if (player.MoveHandler.IsWalking)
        {
            t = new Vector2(player.MoveStatusHandler.Perp.x * _currentSpeed * -player.InputHandler.MoveInput * dt,
                            player.MoveStatusHandler.Perp.y * _currentSpeed * -player.InputHandler.MoveInput * dt);
        }

        if (player.InputHandler.DashRequested && player.MoveStatusHandler.IsGround)
        {
            player.ChangeMoveState(MoveBehavior.Dash);
        }
    }

    public override void FixedExecute(MainPlayer player)
    {
        if (player.MoveHandler.IsWalking)
        {
            player.transform.Translate(t, Space.World);
        }

        if (player.InputHandler.JumpRequested && player.MoveStatusHandler.IsGround)
        {
            player.Rigidbody2D.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
            player.InputHandler.UseJumpRequest();
        }
    }

    public override void Exit(MainPlayer player)
    {
        if (!player.MoveHandler.IsWalking)
        {
            player.AnimatorManager?.SetMoveBool(false);
        }
        _currentSpeed = 0f;
        _walkTimer = 0f;
    }
}

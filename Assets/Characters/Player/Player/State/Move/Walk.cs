using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Walk : State<MainPlayer>
{
    private float _maxWalkSpeed;
    private float _walkAccelTime;
    private float _currentSpeed;

    
    private float _walkTimer;
    private float dt = Time.deltaTime;
    private Vector2 t;

    public float CurrentSpeed => _currentSpeed;


    public Walk(float maxWalkSpeed, float walkAccelTime)
    {
        _walkAccelTime = walkAccelTime;
        _maxWalkSpeed = maxWalkSpeed;
    }

    public override void Enter(MainPlayer player)
    {
        Debug.Log("Walk ÁøÀÔ");

        if (player.WalkHandler.RemainSpeed > 0)
        {
            _currentSpeed = player.WalkHandler.RemainSpeed;
            _walkTimer = 0f;
        } 
        else
        {
            _currentSpeed = 0f;
            _walkTimer = 0f;
        }
    }

    public override void Execute(MainPlayer player)
    {
        if (!player.WalkHandler.IsWalking)
            player.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            player.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;


        if (player.WalkHandler.IsWalking)
        {
            _walkTimer += dt;
            float t = Mathf.Clamp01(_walkTimer / _walkAccelTime);
            _currentSpeed = Mathf.Lerp(0f, _maxWalkSpeed, t);
        }
        else
        {
            _walkTimer = 0f;
            _currentSpeed = 0f;
        }

        if (player.WalkHandler.IsWalking)
        { 
            t = new Vector2(player.MoveStatusHandler.Perp.x * _currentSpeed * player.InputHandler.MoveInput * dt,
                            player.MoveStatusHandler.Perp.y * _currentSpeed * player.InputHandler.MoveInput * dt);
        }

        if(player.InputHandler.DashRequested && player.MoveStatusHandler.IsGround)
        {
            player.ChangeMoveState(MoveBehavior.Dash);
        }

        Debug.Log(_currentSpeed);
    }

    public override void FixedExecute(MainPlayer player)
    {
        if (player.MoveStatusHandler.IsSlope && player.MoveStatusHandler.IsGround)
            player.Rigidbody2D.linearVelocity = Vector2.zero;

        if (player.WalkHandler.IsWalking)
        {
            player.transform.Translate(t, Space.World);
        }
    }

    public override void Exit(MainPlayer player)
    {
        _currentSpeed = 0f;
        _walkTimer = 0f;
    }
}

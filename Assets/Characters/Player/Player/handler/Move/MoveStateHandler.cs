using UnityEngine;

public class MoveStateHandler : MonoBehaviour
{
    private MainPlayer _player;
    private bool _isWalking, _isDashing, _isJumping, _canDash, _preValue;
    private float _dashTimer;
    public void Initialize(MainPlayer player)
    {
        _player = player;
    }

    public void Update()
    {
        if(!_canDash)
        {
            if (!_canDash && _isDashing == false)
            {
                _dashTimer += Time.deltaTime;
                if (_dashTimer >= 3f)
                {
                    _canDash = true;
                }
            }
        }
    }
    public void MoveState()
    {
        if (!_player.PlayerHpHandler.IsHeating)
        {
            if (_player.InputHandler.DashRequested && _player.MoveStatusHandler.CanJump && !_isDashing && _canDash)
            {
                _player.ChangeMoveState(MoveBehavior.Dash);

            }
            else if (_player.InputHandler.JumpRequested && _player.MoveStatusHandler.CanJump)
            {
                _player.ChangeMoveState(MoveBehavior.Jump);
            }
        } 
        else
        {
            _player.ChangeMoveState(MoveBehavior.Idle);
        }
    }

    public void EventState()
    {

    }


    public void StartDash()
    {
        _canDash = false;
        _isDashing = true;
    }

    public void StartDashCooltime()
    {
        _isDashing = false;
        _dashTimer = 0f;
    }
}

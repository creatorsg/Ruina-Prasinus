using UnityEngine;

public class MoveStateHandler : MonoBehaviour
{
    private MainPlayer _player;

    public void Initialize(MainPlayer player)
    {
        _player = player;
    }

    public void MoveState()
    {
        if (_player.InputHandler.DashRequested && _player.MoveStatusHandler.CanJump && _player.InputHandler.IsDashHeld)
        {
            _player.ChangeMoveState(MoveBehavior.Dash);

        }
        else if (_player.InputHandler.JumpRequested && _player.MoveStatusHandler.CanJump)
        {
            _player.ChangeMoveState(MoveBehavior.Jump);
        }
    }
}

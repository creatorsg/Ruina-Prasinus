using UnityEngine;

public class Enemy3Stop : State<PurpleMushrooms>
{
    private float _stopTimer;
    private float _stopDuration = 2f; 

    public override void Enter(PurpleMushrooms enemy3)
    {
        _stopTimer = 0f;
        Debug.Log("멈춤");
    }

    public override void Execute(PurpleMushrooms enemy3)
    {
        _stopTimer += Time.deltaTime;

        if (_stopTimer >= _stopDuration)
        {
            enemy3.MoveHandler.ChangeDirection();
            enemy3.ChangeState(Enemy3Behaviour.Idle);
        }
    }

    public override void FixedExecute(PurpleMushrooms enemy3) { }
    public override void Exit(PurpleMushrooms enemy3) { }
}
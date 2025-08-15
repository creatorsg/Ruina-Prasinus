using UnityEngine;

public class Enemy3Rush : State<PurpleMushrooms>
{
    private float _rushSpeed, _standtimer;
    public Enemy3Rush(float rushSpeed)
    {
        _rushSpeed = rushSpeed;
    }
    public override void Enter(PurpleMushrooms enemy3)
    {
        _standtimer = 0f;

        // 놀라는 애니메이션 약 (0.5초)
    }

    public override void Execute(PurpleMushrooms enemy3)
    {
        if(_standtimer > 0.5f)
        {
            
        }
    }
    public override void FixedExecute(PurpleMushrooms enemy3)
    {
        if (_standtimer <= 0.5f)
        {
            _standtimer += Time.deltaTime;
        }
    }
    public override void Exit(PurpleMushrooms enemy3)
    {

    }
}

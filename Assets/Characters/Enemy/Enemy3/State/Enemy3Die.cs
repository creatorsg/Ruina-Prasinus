using UnityEngine;

public class Enemy3Die : State<PurpleMushroom>
{
    public enum Phase { stand ,explode }
    private Phase _currentPhase;
    private float _timer;

    
    public override void Enter(PurpleMushroom enemy3)
    {

        
        Debug.Log("Æø¹ß ÁøÀÔ");
        if(enemy3.Enemy3StateHandler.IsHitWall)
        {
            _currentPhase = Phase.explode;
        } else
        {
            _currentPhase = Phase.stand;
        }
        _timer = 0;
    }

    public override void Execute(PurpleMushroom enemy3)
    {
        switch (_currentPhase)
        {
            case Phase.stand:
                _timer += Time.deltaTime;
                if(_timer >= 1f)
                {
                    _currentPhase = Phase.explode;
                }
                break;

            case Phase.explode:
                enemy3.Enemy3StateHandler.Explode();
                break;
        }
    }

    public override void FixedExecute(PurpleMushroom enemy3)
    {
        
    }

    public override void Exit(PurpleMushroom enemy3)
    {

    }
}

using Unity.VisualScripting;
using UnityEngine;

public class GroundEnemyMoveHandler : MonoBehaviour
{
    private PurpleMushrooms _enemy3;
    
    private SlopeCheckHandler _slopeCheckHandler;
    private CliffCheckHandler _cliffCheckHandler;

    public void Initialize(PurpleMushrooms enemy3)
    {
        _enemy3 = enemy3;
    }

    private void Awake()
    {
        _slopeCheckHandler = GetComponent<SlopeCheckHandler>();
        _cliffCheckHandler = GetComponent<CliffCheckHandler>();
    }

    public void GroundCheck()
    {
        if(_cliffCheckHandler.IsCliff)
        {
            _enemy3.ChangeState(Enemy3Behaviour.Stop);
        }
    }
}
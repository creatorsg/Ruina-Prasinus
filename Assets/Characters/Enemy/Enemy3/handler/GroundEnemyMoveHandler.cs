using Unity.VisualScripting;
using UnityEngine;

public class GroundEnemyMoveHandler : MonoBehaviour
{
    private PurpleMushrooms _enemy3;
    
    private SlopeCheckHandler _slopeCheckHandler;
    private CliffCheckHandler _cliffCheckHandler;
    private bool _isGround;
    private LayerMask _groundMask;

    public bool IsGround => _isGround;

    public void Initialize(PurpleMushrooms enemy3)
    {
        _enemy3 = enemy3;
    }

    private void Awake()
    {
        _groundMask = LayerMask.GetMask("Ground");

        _slopeCheckHandler = GetComponent<SlopeCheckHandler>();
        _cliffCheckHandler = GetComponent<CliffCheckHandler>();
    }

    private void Update()
    {
        _isGround = Physics2D.OverlapCircle(gameObject.transform.position, 0.8f, _groundMask);
    }
    public void GroundCheck()
    {
        if(_cliffCheckHandler.IsCliff && _isGround)
        {
            _enemy3.ChangeState(Enemy3Behaviour.Stop);
        }
    }
}
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class MoveStatusHandler : MonoBehaviour
{
    private MainPlayer _player;

    private RaycastHit2D hit, fronthit;
    private GameObject _realMovement;
    private LayerMask _groundMask;

    private bool _isGround, _isSlope, _isJump;
    private Vector2 _perp;
    private float _angle, _jumpTimer;

    public GameObject RealMovement => _realMovement;
    public Vector2 Perp => _perp;
    public bool IsGround => _isGround;
    public bool IsSlope => _isSlope;
    public bool IsJump => _isJump;

    public void Initialize(MainPlayer player)
    {
        _player = player;
    }

    private void Awake()
    {
        _realMovement = GameObject.Find("RealMove");
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        RayCheck();

        if (hit || fronthit)
        {
            if (fronthit)
                SlopeCheck(fronthit);
            else if (hit)
                SlopeCheck(hit);
        }

        if (IsGround)
        {
            _player.AnimatorManager?.SetGroundBool(true);
        }
        else
        {
            _player.AnimatorManager?.SetGroundBool(false);
        }

        if (_player.InputHandler.JumpRequested && _player.MoveStatusHandler.IsGround)
        {
            _isJump = true;
        }

        StartJumpTimer();
        Debug.Log(_isJump);
    }

    public void RayCheck()
    {
        _isGround = Physics2D.OverlapCircle(gameObject.transform.position, 0.5f, _groundMask);
        hit = Physics2D.Raycast(_realMovement.transform.position, Vector2.down, 1f, _groundMask);
        fronthit = Physics2D.Raycast(gameObject.transform.position, transform.right, 0.1f, _groundMask);
    }
    
    public void StartJumpTimer()
    {
        if(_jumpTimer <= 3.5f && _isJump == true)
        {
            _jumpTimer += Time.deltaTime;
        }
        else
        {
            _jumpTimer = 0f;
            _isJump = false;
        }
    }
    public void SlopeCheck(RaycastHit2D hit)
    {
        _perp = Vector2.Perpendicular(hit.normal);
        _angle = Vector2.Angle(hit.normal, Vector2.up);

        if (_angle != 0)
            _isSlope = true;
        else
            _isSlope = false;
    }
}

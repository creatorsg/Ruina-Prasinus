using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class MoveStatusHandler : MonoBehaviour
{
    private MainPlayer _player;

    private RaycastHit2D hit, fronthit;
    private GameObject _realMovement;
    private LayerMask _groundMask;

    private bool _isGround, _isSlope, _canJump;
    private Vector2 _perp;
    private float _angle, _jumpTimer;

    public GameObject RealMovement => _realMovement;
    public Vector2 Perp => _perp;
    public bool IsGround => _isGround;
    public bool IsSlope => _isSlope;
    public bool CanJump => _canJump;

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

        if (_isGround)
        {
            _player.AnimatorManager?.SetGroundBool(true);
        }
        else
        {
            _player.AnimatorManager?.SetGroundBool(false);
        }
    }
    
    public void RayCheck()
    {
        _canJump = Physics2D.BoxCast(transform.position, new Vector2(0.8f, 0.125f), 0, Vector2.down, 0.4f, _groundMask);
        _isGround = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1f, _groundMask);
        hit = Physics2D.Raycast(_realMovement.transform.position, Vector2.down, 1f, _groundMask);
        fronthit = Physics2D.Raycast(gameObject.transform.position, transform.right, 0.1f, _groundMask);
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


    private void OnDrawGizmos()
    {
        Vector2 startPosition = transform.position;
        Vector2 boxSize = new Vector2(0.8f, 0.125f);
        Vector2 endPosition = startPosition + (Vector2.down * 0.4f);

        Gizmos.color = _canJump ? UnityEngine.Color.green : UnityEngine.Color.red;

        Gizmos.DrawWireCube(startPosition, boxSize);
        Gizmos.DrawWireCube(endPosition, boxSize);
        Gizmos.DrawLine(startPosition, endPosition);
    }
}

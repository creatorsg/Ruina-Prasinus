using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class MoveStatusHandler : FindChildObject
{
    private MainPlayer _player;

    private RaycastHit2D hit, fronthit, hit2, fronthit2;
    private Transform _realMovement, _realMovement2;
    private LayerMask _groundMask;

    private bool _isGround, _isSlope, _canJump;
    private Vector2 _perp;
    private float _angle, _jumpTimer;
    private RaycastHit2D _targetHit = default;

    public Transform RealMovement => _realMovement;
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
        _realMovement = FindChildWithTag(transform, "SlopeCheck");
        _realMovement2 = FindChildWithTag(transform, "SlopeCheck2");
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        RayCheck();

        RaycastHit2D targetHit = default;

        if (hit && hit2)
        {
            targetHit = hit.point.y > hit2.point.y ? hit : hit2;
        }
        else if (hit)
        {
            targetHit = hit;
        }
        else if (hit2)
        {
            targetHit = hit2;
        }

        if (targetHit)
        {
            SlopeCheck(targetHit);
        }
        else 
        {
            _isSlope = false;
            _angle = 0;
        }

        if (targetHit)
        {
            SlopeCheck(targetHit);
        }

        _player.AnimatorManager?.SetGroundBool(_isGround);
        Debug.Log(_angle);
        Debug.Log(targetHit);
    }
    
    public void RayCheck()
    {
        _canJump = Physics2D.BoxCast(transform.position, new Vector2(0.8f, 0.155f), 0, Vector2.down, 0.4f, _groundMask);
        _isGround = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1f, _groundMask);
        
        hit = Physics2D.Raycast(_realMovement.transform.position, Vector2.down, 1f, _groundMask);
        hit2 = Physics2D.Raycast(_realMovement2.position, Vector2.down, 1f, _groundMask);
    }

    public void SlopeCheck(RaycastHit2D hit)
    {
        _perp = Vector2.Perpendicular(hit.normal);
        _angle = Vector2.Angle(hit.normal, Vector2.up);

        if (_angle > 0.1f && _angle < 60f)
            _isSlope = true;
        else
            _isSlope = false;
    }


    private void OnDrawGizmos()
    {
        Vector2 startPosition = transform.position;
        Vector2 boxSize = new Vector2(0.8f, 0.155f);
        Vector2 endPosition = startPosition + (Vector2.down * 0.4f);

        Gizmos.color = _canJump ? UnityEngine.Color.green : UnityEngine.Color.red;

        Gizmos.DrawWireCube(startPosition, boxSize);
        Gizmos.DrawWireCube(endPosition, boxSize);
        Gizmos.DrawLine(startPosition, endPosition);
    }
}

using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy3DetectHandler : FindChildObject
{
    private PurpleMushroom _enemy3;
    private RaycastHit2D _hit, _hit2;

    private bool _isCliff, _isSlope, _playerCheck;
    private LayerMask _groundMask, _playerMask;
    private Transform _realMovement, _realMovement2;
    private Vector2 _perp;
    private float _angle;

    public Vector2 Perp => _perp;
    public bool IsCliff => _isCliff;
    public bool PlayerCheck => _playerCheck;
    public void Initialize(PurpleMushroom enemy3)
    {
        _enemy3 = enemy3;
    }

    private void Awake()
    {
        _realMovement = FindChildWithTag(transform, "SlopeCheck");
        _realMovement2 = FindChildWithTag(transform, "SlopeCheck2");
        _groundMask = LayerMask.GetMask("Ground");
        _playerMask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        RayCheck();

        RaycastHit2D targetHit = default;

        if (_hit && _hit2)
        {
            targetHit = _hit.point.y > _hit2.point.y ? _hit : _hit2;
        }
        else if (_hit)
        {
            targetHit = _hit;
        }
        else if (_hit2)
        {
            targetHit = _hit2;
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
    }
    public void RayCheck()
    {
        Vector2 dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

        _isCliff = Physics2D.Raycast(transform.position, Vector2.down, 1f, _groundMask);
        _playerCheck = Physics2D.Raycast(transform.position, dir, 10f, _playerMask);

        _hit = Physics2D.Raycast(_realMovement.transform.position, Vector2.down, 1f, _groundMask);
        _hit2 = Physics2D.Raycast(_realMovement2.position, Vector2.down, 1f, _groundMask);

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
        Vector2 dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, dir * 10f);
    }
}

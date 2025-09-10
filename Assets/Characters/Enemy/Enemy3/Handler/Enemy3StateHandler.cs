using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy3StateHandler : FindChildObject
{
    private PurpleMushroom _enemy3;

    private RaycastHit2D _hit, _hit2;
    private LayerMask _groundMask, _playerMask;
    private Transform _realMovement, _realMovement2;
    private Vector2 _perp, _dir;
    private float _angle, _detectDistance;
    private GameObject _explosion;

    private bool _isCliff, _isSlope, _playerCheck, _isMoving, _isGround;

    public Vector2 Perp => _perp;
    public float Dir => _dir == Vector2.right ? 1 : -1;
    public bool IsMoving => _isMoving;
    public bool IsCliff => _isCliff;
    public bool PlayerCheck => _playerCheck;
    public void Initialize(PurpleMushroom enemy3, float detectDistance)
    {
        _enemy3 = enemy3;
        _detectDistance = detectDistance;
    }

    private void Awake()
    {
        _realMovement = FindChildWithTag(transform, "SlopeCheck");
        _realMovement2 = FindChildWithTag(transform, "SlopeCheck2");
        _groundMask = LayerMask.GetMask("Ground");
        _playerMask = LayerMask.GetMask("Player");

        _explosion = Resources.Load<GameObject>("Circle");
    }

    private void Update()
    {
        _dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        
        DetectPlayer();
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
        _isCliff = Physics2D.Raycast(transform.position, Vector2.down, 1f, _groundMask);
        _playerCheck = Physics2D.Raycast(transform.position, _dir, 10f, _playerMask);

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

    public void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _detectDistance, _playerMask);

        if (hit != null)
        {
             _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }

    public void Explode()
    {
        GameObject obj = Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(obj);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 20f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _dir * 10f);
    }
}

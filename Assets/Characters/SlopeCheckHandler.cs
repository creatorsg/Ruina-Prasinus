using UnityEngine;

public class SlopeCheckHandler : FindChildObject
{
    private LayerMask _groundMask;
    private Transform _slopeCheck;
    private RaycastHit2D hit, fronthit;
    private float _angle;
    private bool _isSlope;
    public bool IsSlope => _isSlope;

    private void Awake()
    {
        _groundMask = LayerMask.GetMask("Ground");
        _slopeCheck = FindChildWithTag(transform, "SlopeCheck");
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
    }

    public void RayCheck()
    {
        hit = Physics2D.Raycast(_slopeCheck.transform.position, Vector2.down, 1f, _groundMask);
        fronthit = Physics2D.Raycast(_slopeCheck.transform.position, transform.right, 0.1f, _groundMask);
    }

    private void SlopeCheck(RaycastHit2D hit)
    {
        _angle = Vector2.Angle(hit.normal, Vector2.up);

        if (_angle != 0)
            _isSlope = true;
        else
            _isSlope = false;
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class GroundEnemyMoveHandler : MonoBehaviour
{
    private PurpleMushrooms _enemy3;
    private RaycastHit2D hit, fronthit;

    private LayerMask _groundMask;
    private bool _isCliff, _isSlope;
    private Transform _slopeCheck, _chiffCheck;

    private float _angle;

    public void Initialize(PurpleMushrooms enemy3)
    {
        _enemy3 = enemy3;
    }

    private void Awake()
    {
        _groundMask = LayerMask.GetMask("Ground");
        _slopeCheck = FindChildWithTag(transform, "SlopeCheck");
        _chiffCheck = FindChildWithTag(transform, "ChiffCheck");

    }

    private void Update()
    {
        RayCheck();
        ChiffCheck();

        if (hit || fronthit)
        {
            if (fronthit)
                SlopeCheck(fronthit);
            else if (hit)
                SlopeCheck(hit);
        }


        Debug.Log($"절벽 감지: {_isCliff}, 벽 감지: {_isSlope}");
    }

    public void RayCheck()
    {
        hit = Physics2D.Raycast(_slopeCheck.transform.position, Vector2.down, 1f, _groundMask);
        fronthit = Physics2D.Raycast(_slopeCheck.transform.position, transform.right, 0.1f, _groundMask);
    }

    public void SlopeCheck(RaycastHit2D hit)
    {
        _angle = Vector2.Angle(hit.normal, Vector2.up);

        if (_angle != 0)
            _isSlope = true;
        else
            _isSlope = false;
    }
    
    public void ChiffCheck()
    {
        _isCliff = !Physics2D.OverlapCircle(_chiffCheck.transform.position, 0.1f, _groundMask);
    }

    private Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        return null;
    }

}
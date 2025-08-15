using UnityEngine;

public class CliffCheckHandler : FindChildObject
{
    private LayerMask _groundMask;
    private Transform _cliffCheck;
    private bool _isCliff;

    public bool IsCliff => _isCliff;

    private void Awake()
    {
        _groundMask = LayerMask.GetMask("Ground");
        _cliffCheck = FindChildWithTag(transform, "ChiffCheck");
    }

    private void Update()
    {
        CliffCheck(); 
    }

    public void CliffCheck()
    {
        _isCliff = !Physics2D.OverlapCircle(_cliffCheck.transform.position, 0.1f, _groundMask);
    }
}

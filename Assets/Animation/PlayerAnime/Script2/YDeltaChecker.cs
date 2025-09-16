using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class YDeltaChecker : MonoBehaviour
{
    private MainPlayer _player;
    public enum UpDownState { Up, Down};
    private float previousY;
    private float DeltaY { get; set; }
    public UpDownState CurrentState { get; private set; }

    private bool _isfalling;

    public bool IsFalling => _isfalling;


    public void Initialize(MainPlayer player)
    {
        _player = player;
    }
    void Start()
    {
        previousY = transform.position.y;
    }

    void Update()
    {
        float currentY = transform.position.y;
        DeltaY = currentY - previousY;
        previousY = currentY;
        CurrentState = (DeltaY > 0) ? UpDownState.Up : UpDownState.Down;

        if (CurrentState == UpDownState.Down)
        {
            _isfalling = true;
        }
        else
        {
            _isfalling = false;
        }
    }
}

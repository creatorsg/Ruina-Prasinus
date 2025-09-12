using System;
using UnityEngine;

public class YDeltaChecker : MonoBehaviour
{
    public enum UpDownState { Up, Down};
    private float previousY;
    private float DeltaY { get; set; }
    public UpDownState CurrentState { get; private set; }

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
    }
}

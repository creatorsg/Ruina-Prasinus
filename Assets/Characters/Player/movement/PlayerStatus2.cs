using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatus2
{
    public bool isGround { get; private set; }
    public bool isSlope { get; private set; }

    public Vector2 perp;
    private float angle;

    public void Update(Transform Player, float checkRadius, LayerMask groundMask)
    {
        GroundCheck(Player, checkRadius, groundMask);
    }
    void GroundCheck(Transform Player, float checkRadius, LayerMask ground)
    {
        isGround = Physics2D.OverlapCircle(Player.position, checkRadius, ground);
        Debug.Log(isGround);
    }

    public void SlopeCheck(RaycastHit2D hit)
    {
        perp = Vector2.Perpendicular(hit.normal);
        angle = Vector2.Angle(hit.normal, Vector2.up);

        Debug.Log("히트 체크중");

        if (angle != 0)
            isSlope = true;
        else
            isSlope = false;
    }
}

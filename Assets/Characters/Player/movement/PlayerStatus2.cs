using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Playerstatus2
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
    }

    public void SlopeCheck(RaycastHit2D hit)
    {
        perp = Vector2.Perpendicular(hit.normal);
        angle = Vector2.Angle(hit.normal, Vector2.up);

        if (angle != 0)
            isSlope = true;
        else
            isSlope = false;
    }
}

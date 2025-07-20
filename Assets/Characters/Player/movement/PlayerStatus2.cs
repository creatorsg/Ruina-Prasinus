using JetBrains.Annotations;
using UnityEngine;

public class PlayerStatus2
{

    public bool isGround { get; private set; }
    public bool isHit { get; private set; }
    public bool isInvincible { get; private set; }

    public void Update(Transform Player, float checkRadius, LayerMask groundMask)
    {
        GroundCheck(Player, checkRadius, groundMask);

    }
    void GroundCheck(Transform Player, float checkRadius, LayerMask ground)
    {
        isGround = Physics2D.OverlapCircle(Player.position, checkRadius, ground);
    }

    void HitStart(PlayerState state, Enemy enemy)
    {

    }
}

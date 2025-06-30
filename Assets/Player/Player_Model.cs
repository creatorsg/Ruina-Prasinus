using UnityEngine;
using System.Collections;
public class Player_Model
{
    //플레이어 현재 상태 
    public bool canDash = true;
    public bool isGrounded = false;
    public bool isWalking = false;
    public bool isDashing = false;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isHit = false;
    public bool inCinematic = false;
    public bool inGearSetting = false;
    public bool inGearAction = false;
    public bool inUIControl = false;
}

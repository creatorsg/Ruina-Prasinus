using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Scriptable Objects/PlayerState")]
public class PlayerState : ScriptableObject
{
    [Header("— 스탯 (Status) —")]
    public float hp = 500f;
    public readonly float attack_power = 5f;

    [Header("걷기")]
    public readonly float walkAccelTime = 0.1f;  
    public readonly float walkMaxSpeed = 5f;

    [Header("대쉬")]
    public readonly float dashAccelTime = 0.2f; 
    public readonly float dashMaintainTime = 0.3f;  
    public readonly float dashMaxSpeed = 7.5f;
    public readonly float dashCooldownTime = 0.2f;  

    [Space(10)]
    [Header("— 점프 (Jump) —")]
    public readonly float jumpPower = 5f;
    public readonly float jumpHoldTime = 0.25f;
    public readonly float jumpDuration = 0.25f;

    [Space(10)]
    [Header("— 피격 (isHit) —")]
    public float invincibleDuration = 3f;
    public float freezeDuration = 0.5f;
}

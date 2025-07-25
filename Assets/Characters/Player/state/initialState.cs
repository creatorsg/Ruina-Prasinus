using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Scriptable Objects/PlayerState")]
public class initialState : ScriptableObject
{
    [Header("— 스탯 (Status) —")]
    public float hp = 500f;
    public float attack_power = 5f;

    [Header("걷기")]
    public float walkAccelTime = 0.1f;  
    public float walkMaxSpeed = 5f;

    [Header("대쉬")]
    public float dashAccelTime = 0.2f; 
    public float dashMaintainTime = 0.3f;  
    public float dashMaxSpeed = 7.5f;
    public float dashCooldownTime = 5f;


    [Space(10)]
    [Header("— 점프 (Jump) —")]
    public float jumpPower = 5f;
    public float jumpHoldTime = 0.25f;
    public float jumpDuration = 0.25f;

    [Space(10)]
    [Header("— 피격 (isHit) —")]
    public float invincibleDuration = 3f;
    public float freezeDuration = 0.5f;
}

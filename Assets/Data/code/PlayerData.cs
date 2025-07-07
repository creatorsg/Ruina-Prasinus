using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("— 스탯 (Status) —")]
    public float hp = 500f;

    [Space(10)]
    [Header("— 걷기 (Walking) —")]
    public float walkMaxSpeed = 5f;
    public float walkAccelTime = 0.1f;
    public float currentWalkSpeed = 0f;

    [Space(10)]
    [Header("— 대시 (Dash) —")]
    public float dashMaxSpeed = 7.5f;
    public float dashAccelTime = 0.2f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 0.2f;
    public float currentDashSpeed = 0f;

    [Space(10)]
    [Header("— 점프 (Jump) —")]
    public float jumpSpeed = 5f;
    public float jumpHoldTime = 0.25f;

    [Space(10)]
    [Header("— 피격 (isHit) —")]
    public float invincibleTimer = 0f;
    public float invincibleDuration = 3f;
    public float freezeTimer = 0f;
    public float freezeDuration = 0.5f;

    [Space(10)]
    [Header("- 공격 (Attack) -")]
    public float attack_power = 5f;

    [Space(10)]
    [Header("- 상태 (State) -")]
    public Tilemap room = null;
}

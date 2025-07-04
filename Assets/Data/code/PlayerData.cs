﻿using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
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
}

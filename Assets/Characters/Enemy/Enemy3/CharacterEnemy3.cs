using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterEnemy3", menuName = "Scriptable Objects/CharacterEnemy3")]
public class CharacterEnemy3 : ScriptableObject
{
    [Header("Idle(Walk) 상태일 때")]
    private float _moveSpeed = 1f;
    private float _detectionRange = 8f;

    [Header("Rush 상태일 때")]
    private float _rushSpeed = 5f;
    private float _exploerDetectDistance = 1f;

    public float MoveSpeed => _moveSpeed;
    public float DetectionRange => _detectionRange;

    public float RushSpeed => _rushSpeed;
}
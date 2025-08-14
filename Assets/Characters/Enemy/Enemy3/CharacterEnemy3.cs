using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterEnemy3", menuName = "Scriptable Objects/CharacterEnemy3")]
public class CharacterEnemy3 : ScriptableObject
{
    [Header("Idle(Walk) 상태일 때")]
    private float _moveSpeed = 3f;

    public float MoveSpeed => _moveSpeed;
}
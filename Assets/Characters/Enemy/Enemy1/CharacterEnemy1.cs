using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterEnemy1", menuName = "Scriptable Objects/CharacterEnemy1")]
public class CharacterEnemy1 : ScriptableObject
{
    [Header("�� �⺻ ����")]
    private float _enemyHp = 30f;

    [Header("None ������ ��")]
    private float _spawnDistance = 5f;

    [Header("Move ������ ��")]
    private float _moveSpeed = 3f;

    [Header("Idle ������ ��")]
    private float _detectDistance = 50f;

    [Header("Attack ������ ��")]
    private float _attackDistance = 2f;
    private float _attackPoewr = 10f;

    public float EnemyHp => _enemyHp;
    public float SpawnDistance => _spawnDistance;
    public float MoveSpeed => _moveSpeed;
    public float DetectDistance => _detectDistance;
    public float AttackDistance => _attackDistance;
    public float AttackPower => _attackPoewr;
}

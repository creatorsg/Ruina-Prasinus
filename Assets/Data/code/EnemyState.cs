using UnityEngine;

[CreateAssetMenu(fileName = "EnemyState", menuName = "Scriptable Objects/EnemyState")]
public class EnemyState : ScriptableObject
{
    [Header("— 적 스탯 구현 —")]
    public float hp = 3f;
    public float attackPower = 5f;
    public float moveSpeed = 3f;
    public float attackRange = 2f;
}
 
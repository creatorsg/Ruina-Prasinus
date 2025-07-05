using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [Header("— 적 스탯 구현 —")]
    public float hp = 3f;
    public float attackPower = 5f;
    public float moveSpeed = 3f;
}

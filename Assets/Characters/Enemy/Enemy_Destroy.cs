using Player;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Destroy : MonoBehaviour
{
    [Header("Data (ScriptableObject)")]
    [SerializeField] private Enemy enemy;

    [Header("— 스폰 관리용 (RoomEnemyRespawner에서 세팅) —")]
    [HideInInspector] public RoomEnemyRespawner roomRespawner;
    [HideInInspector] public int spawnIndex;

    void Awake()
    {
        enemy.hp = 30f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            enemy.hp -= 10;
            if (enemy.hp <= 0)
            {
                if (roomRespawner != null)
                    roomRespawner.MarkDestroyed(spawnIndex);

                Destroy(gameObject);
            }
        }
    }
}
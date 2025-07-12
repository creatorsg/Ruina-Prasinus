using Player;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Destroy : MonoBehaviour
{
    [Header("Data (ScriptableObject)")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Map map;

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

                if (map != null)
                    map.enemy_num -= 1;

                Destroy(gameObject);
            }
        }
    }
}
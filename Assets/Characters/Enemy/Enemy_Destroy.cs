using Player;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Destroy : MonoBehaviour
{
    [Header("Data (ScriptableObject)")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Map map;

    void Awake()
    {
        enemy.hp = 30f;
    }

    [Header("— 스폰 관리용 (RoomEnemyRespawner에서 세팅) —")]
    [HideInInspector] public RoomEnemyRespawner roomRespawner;
    [HideInInspector] public int destroyCheck;

    void Awake()
    {
        enemy.hp = 30f;
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Bullet"))
        {
            enemy.hp -= 10;
            if (enemy.hp <= 0)
            {
                if (roomRespawner != null)
                    roomRespawner.MarkDestroyed(destroyCheck);

                Destroy(gameObject);
                if (map != null)
                {
                    map.enemy_num -= 1;
                }
            }
        }
    }
}
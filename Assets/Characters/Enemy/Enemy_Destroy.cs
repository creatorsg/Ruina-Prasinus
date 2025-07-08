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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            enemy.hp -= 10;

            if (enemy.hp <= 0)
            {
                Destroy(gameObject);
                if (map != null)
                {
                    map.enemy_num -= 1;
                }
            }
        }
    }
}
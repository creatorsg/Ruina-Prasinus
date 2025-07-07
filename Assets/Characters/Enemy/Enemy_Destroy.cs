using UnityEngine;

public class Enemy_Destroy : MonoBehaviour
{
    [Header("Data (ScriptableObject)")]
    [SerializeField] private Enemy enemy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            enemy.hp -= 10;

            if (enemy.hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
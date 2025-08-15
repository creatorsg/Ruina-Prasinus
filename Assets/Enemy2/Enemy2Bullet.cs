using UnityEngine;

public class Enemy2Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}


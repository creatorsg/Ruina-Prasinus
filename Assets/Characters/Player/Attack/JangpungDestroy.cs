using UnityEngine;

public class JangpungDestroy : MonoBehaviour
{
    private float autoDestroyTime = 0.4f;

    void Start()
    {
        Destroy(gameObject, autoDestroyTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

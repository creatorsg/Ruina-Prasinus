using UnityEngine;

public class JangpungDestroy : MonoBehaviour
{
    private float autoDestroyTime = 0.4f;

    void Start()
    {
        Destroy(gameObject, autoDestroyTime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

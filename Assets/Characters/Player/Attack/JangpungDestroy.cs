using UnityEngine;

public class JangpungDestroy : MonoBehaviour
{
    [SerializeField] Animator _animator;
    private float autoDestroyTime = 0.8f;

    void Start()
    {
        Destroy(gameObject, autoDestroyTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Enemy"))
        {
            _animator.SetBool("hasHitWall", true);
            Destroy(gameObject, 0.3f);
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class SporeButterfly : MonoBehaviour
{
    [Header("���� �ݰ�")]
    public float detectionRange = 5f;
    [Header("�̵� �ӵ�")]
    public float speed = 3f;

    private Transform player;
    private bool isChasing = false;

    void Awake()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;
        else Debug.LogError("[SporeButterfly] Player �±׸� ã�� �� �����ϴ�.");
    }

    void Update()
    {
        if (player == null) return;

        if (!isChasing &&
            Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            Vector2 nextPos = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
            transform.position = new Vector3(nextPos.x, nextPos.y, transform.position.z);
        }
    }

    void OnDrawGizmosSelected()
    { 
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

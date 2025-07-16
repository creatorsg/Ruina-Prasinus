using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class SporeButterfly : MonoBehaviour
{
    [Header("공격 반경")]
    public float detectionRange = 5f;
    [Header("이동 속도")]
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
        else Debug.LogError("[SporeButterfly] Player 태그를 찾을 수 없습니다.");
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

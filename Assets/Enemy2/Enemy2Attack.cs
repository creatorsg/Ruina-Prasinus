using UnityEngine;

public class Enemy2Attack : MonoBehaviour
{
    [Header("�߻��� ������")]
    public GameObject projectilePrefab;

    [Header("�߻� �ӵ� (��)")]
    public float launchSpeed = 10f;

    [Header("�߷� �� (Rigidbody2D.gravityScale�� ����)")]
    public float gravity = 9.81f;

    private Transform player;

    void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;
        else Debug.LogError("[EnemyProjectileShooter] Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
    }

    public void ShootAtPlayer()
    {
        if (player == null) return;

        Vector2 startPos = transform.position;
        Vector2 targetPos = player.position;
        Vector2 diff = targetPos - startPos;

        GameObject projectile = Instantiate(projectilePrefab, startPos, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Projectile에 Rigidbody2D가 필요합니다!");
            return;
        }

        float g = Mathf.Abs(Physics2D.gravity.y * (gravity / Physics2D.gravity.magnitude));

        // 방향 부호
        float dirSign = Mathf.Sign(diff.x);

        // 원하는 고각 (예: 75도 → 박격포 스타일)
        float angleDeg = 75f;
        float angle = angleDeg * Mathf.Deg2Rad;

        float distance = Mathf.Abs(diff.x);
        float height = diff.y;

        // 필요한 초기 속도 계산 공식
        // v^2 = g*x^2 / (2*cos^2(angle)*(x*tan(angle) - y))
        float denom = 2 * Mathf.Cos(angle) * Mathf.Cos(angle) * (distance * Mathf.Tan(angle) - height);

        if (denom <= 0)
        {
            Debug.LogWarning("이 각도로는 목표에 도달 불가");
            return;
        }

        float speed = Mathf.Sqrt(g * distance * distance / denom);

        // 속도 벡터
        float vX = Mathf.Cos(angle) * speed * dirSign;
        float vY = Mathf.Sin(angle) * speed;

        rb.linearVelocity = new Vector2(vX, vY);
        rb.gravityScale = gravity / Physics2D.gravity.magnitude;
    }



}

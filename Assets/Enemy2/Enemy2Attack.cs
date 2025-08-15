using UnityEngine;

public class Enemy2Attack : MonoBehaviour
{
    [Header("발사체 프리팹")]
    public GameObject projectilePrefab;

    [Header("포물선 최고 높이 (거리와 무관하게 고정)")]
    public float maxHeight = 3f;

    [Header("중력 비율")]
    public float gravity = 9.81f;

    private Transform player;

    void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;
        else Debug.LogError("[Enemy2Attack] Player를 찾을 수 없습니다.");
    }

    public void ShootAtPlayer()
    {
        if (player == null) return;

        Vector2 startPos = transform.position;
        Vector2 targetPos = player.position;

        float dirSign = Mathf.Sign(targetPos.x - startPos.x);
        float g = Mathf.Abs(Physics2D.gravity.y * (gravity / Physics2D.gravity.magnitude));

        // 수평 거리
        float dx = Mathf.Abs(targetPos.x - startPos.x);
        // 높이 차
        float dy = targetPos.y - startPos.y;

        // 원하는 최고점에 맞는 초기 Y속도
        float vY = Mathf.Sqrt(2 * g * maxHeight);

        // 비행 시간 계산: 올라가는 시간 + 내려오는 시간
        float tUp = vY / g;
        float tDown = Mathf.Sqrt(2 * (maxHeight - dy) / g);
        float totalTime = tUp + tDown;

        // 가로 속도
        float vX = dx / totalTime * dirSign;

        // 발사체 생성
        GameObject projectile = Instantiate(projectilePrefab, startPos, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Projectile에 Rigidbody2D가 필요합니다!");
            return;
        }

        rb.gravityScale = gravity / Physics2D.gravity.magnitude;
        rb.linearVelocity = new Vector2(vX, vY);
    }
}

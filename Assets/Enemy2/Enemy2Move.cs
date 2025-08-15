using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy2Move : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 2f;       // 좌우 이동 속도
    public float stopDuration = 0.5f;  // 방향 전환 시 멈추는 시간

    [Header("이동 방향")]
    public int direction = 1;           // 1 = 오른쪽, -1 = 왼쪽 (Inspector에서 지정)

    private Rigidbody2D rb;
    private bool isStopped;

    // Detect 스크립트 참조
    private Enemy2Detect detectScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        FlipSprite(); // 초기 방향 적용

        detectScript = GetComponent<Enemy2Detect>();
        if (detectScript == null)
            Debug.LogWarning("[Enemy2Move] Enemy2Detect 컴포넌트를 찾을 수 없습니다.");
    }

    private void FixedUpdate()
    {
        // 발사 직후 잠깐 멈춤
        if (detectScript != null && detectScript.StopMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 방향 전환 중 멈춤
        if (isStopped)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 이동
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    private IEnumerator StopForSeconds(float seconds)
    {
        isStopped = true;
        yield return new WaitForSeconds(seconds);
        isStopped = false;
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}

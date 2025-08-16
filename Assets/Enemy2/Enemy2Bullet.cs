using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class DestroyOnHit : MonoBehaviour
{
    // 트리거 충돌
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ShouldDestroy(other)) Destroy(gameObject);
    }

    // 일반(비트리거) 충돌
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDestroy(collision.collider)) Destroy(gameObject);
    }

    // 파괴 조건 판단
    private bool ShouldDestroy(Collider2D col)
    {
        // 1) realplayer 태그와 충돌
        if (col.CompareTag("realplayer")) return true;

        // 2) ground 태그의 TilemapCollider2D와 충돌
        //    (타일맵 오브젝트에 TilemapCollider2D 컴포넌트와 ground 태그가 있어야 합니다)
        if (col.CompareTag("Ground") && col is TilemapCollider2D) return true;

        return false;
    }
}

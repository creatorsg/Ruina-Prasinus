using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class DestroyOnHit : MonoBehaviour
{
    // 트리거 충돌
    private MonsterSound _sound;
    public event Action<GameObject> OnDestroyed;

    private void Awake()
    {
        _sound = GetComponent<MonsterSound>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ShouldDestroy(other))
        {
            OnDestroyed?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    // 일반(비트리거) 충돌
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDestroy(collision.collider))
        {
            OnDestroyed?.Invoke(gameObject);
            _sound.PlayExplodeSound();
            Destroy(gameObject);
        }
    }

    // 파괴 조건 판단
    private bool ShouldDestroy(Collider2D col)
    {

        if (col.CompareTag("PlayerBody")) return true;

        // 2) ground 태그의 TilemapCollider2D와 충돌
        //    (타일맵 오브젝트에 TilemapCollider2D 컴포넌트와 ground 태그가 있어야 합니다)
        if (col.CompareTag("Ground") && col is TilemapCollider2D) return true;

        return false;
    }
}

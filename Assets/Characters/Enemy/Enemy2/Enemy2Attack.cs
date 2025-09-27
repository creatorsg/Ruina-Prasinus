using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy2Attack : MonoBehaviour
{
    private EnemyAttackHandler _attackHandler;
    private SpriteRenderer _spriteRenderer;
    private float _hp;
    private GameObject _explosion;
    private float _timer;

    [Header("발사체 프리팹")]
    public GameObject projectilePrefab;

    [Header("포물선 최고 높이 (거리와 무관하게 고정)")]
    public float maxHeight = 3f;

    [Header("중력 비율")]
    public float gravity = 9.81f;

    private Transform player;

    private void Awake()
    {
        _attackHandler = GetComponent<EnemyAttackHandler>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _explosion = Resources.Load<GameObject>("DieEffect");

        _hp = 20f;
    }

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

        // 발사 위치를 위로 약간 올리기 (예: 1 유닛)
        float offsetY = 1f;
        startPos.y += offsetY;

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
        rb.linearVelocity = new Vector2(vX, vY); // linearVelocity → velocity로 수정
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {

            StartCoroutine(Blink());
            _attackHandler.Damaged(_hp, 5f);
            _hp = _hp - 5f;
            if (_hp <= 0f)
            {
                Explode();
                Destroy(gameObject);
            }
        }
    }
    public IEnumerator Blink()
    {
        Color originalColor = _spriteRenderer.color;
        _spriteRenderer.color = new Color(3f, 3f, 3f, 1f);
        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = originalColor;
    }
    public void Explode()
    {
        _spriteRenderer.enabled = false;
        GameObject obj = Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(obj, 0.8f);
    }

}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Enemy2Attack))]
public class Enemy2Controller : MonoBehaviour
{
    [Header("발사 설정")]
    public float detectionRange = 10f;
    public float shootInterval = 0.5f;
    public float stopMoveDuration = 1f;
    public float preShootStopDuration = 1f; // 최초 발사 전 대기 시간

    [Header("움직임")]
    public float moveSpeed = 1f;
    public float moveTime = 2f;
    public float stopDuration = 0.5f;
    public int direction = 1;

    private float moveTimer = 0f;

    private Rigidbody2D rb;
    private Enemy2Attack shooter;
    private Transform player;
    private bool isChasing = false;
    private bool isStopped = false;
    [HideInInspector] public bool StopMove = false; // �߻� �� ��� ����

    private TimerHandler timerHandler = new TimerHandler();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shooter = GetComponent<Enemy2Attack>();
        FlipSprite();
        FindPlayer();
    }

    private void Update()
    {
        if (player == null) { FindPlayer(); return; }

        float dist = Vector2.Distance(transform.position, player.position);

        // �÷��̾� ����/��Ż
        if (!isChasing && dist <= detectionRange)
        {
            isChasing = true;
            StartShooting();
        }
        else if (isChasing && dist > detectionRange)
        {
            isChasing = false;
            StopMove = false;
            timerHandler.ClearAllTimers();
        }

        // TimerHandler ������Ʈ
        timerHandler.UpdateTimers(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // 발사 후 멈춤
        if (StopMove || isStopped)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // 방향 전환 타이머
        moveTimer += Time.fixedDeltaTime;
        if (moveTimer >= moveTime)
        {
            moveTimer = 0f;

            // 방향 전환
            direction *= -1;
            FlipSprite();

            // 방향 전환 시 멈춤
            StopForDirectionChange();
        }
    }


    private void StartShooting()
    {
        if (!isChasing) return;

        // 최초 발사 전 대기 → StopMove = true
        StopMove = true;
        timerHandler.AddTimer(preShootStopDuration, () =>
        {
            // 멈춤 끝 → 바로 발사
            StopMove = false;
            shooter.ShootAtPlayer();

            // 이후 반복 발사 시작
            timerHandler.AddTimer(shootInterval, ShootRepeat, repeat: true);
        });
    }

    // 반복 발사 콜백
    private void ShootRepeat()
    {
        if (!isChasing) return;

        // 발사 전 잠깐 멈춤
        StopMove = true;
        timerHandler.AddTimer(preShootStopDuration, () =>
        {
            StopMove = false;
            shooter.ShootAtPlayer();
        });
    }



    // ���� ��ȯ �� ���� ȣ��
    public void StopForDirectionChange()
    {
        if (!isStopped)
        {
            isStopped = true;
            timerHandler.AddTimer(stopDuration, () => isStopped = false);
        }
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void FindPlayer()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;
    }

    private void OnDrawGizmosSelected()
    {
        // 감지 범위 시각화
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

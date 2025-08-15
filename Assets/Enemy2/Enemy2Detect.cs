using UnityEngine;

[RequireComponent(typeof(Enemy2Attack))]
public class Enemy2Detect : MonoBehaviour
{
    [Header("감지 설정")]
    public float detectionRange = 5f;

    [Header("발사 설정")]
    public float shootInterval = 1f;          // 발사 전 쿨타임
    public float stopMoveDuration = 0.2f;     // 발사 후 잠깐 멈춤

    [HideInInspector] public bool StopMove = false;

    private Transform player;
    private bool isChasing = false;
    private Enemy2Attack shooter;

    private TimerHandler timerHandler = new TimerHandler();

    private void Awake()
    {
        shooter = GetComponent<Enemy2Attack>();
        FindPlayer();
    }

    private void Update()
    {
        if (player == null) { FindPlayer(); return; }

        float dist = Vector2.Distance(transform.position, player.position);

        // 플레이어 감지/이탈
        if (!isChasing && dist <= detectionRange)
        {
            isChasing = true;
            StartShooting();
        }
        else if (isChasing && dist > detectionRange)
        {
            isChasing = false;
            StopMove = false;
            timerHandler.ClearAllTimers(); // 감지 해제 시 모든 타이머 초기화
        }

        timerHandler.UpdateTimers(Time.deltaTime);
    }

    private void StartShooting()
    {
        // 발사 반복 타이머
        timerHandler.AddTimer(shootInterval, () =>
        {
            if (!isChasing) return;

            // 발사
            shooter.ShootAtPlayer();

            // 발사 후 잠깐 이동 멈춤
            StopMove = true;
            timerHandler.AddTimer(stopMoveDuration, () => StopMove = false);

        }, repeat: true);
    }

    private void FindPlayer()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;
    }
}

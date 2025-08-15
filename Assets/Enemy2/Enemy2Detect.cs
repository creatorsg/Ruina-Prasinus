using UnityEngine;

[RequireComponent(typeof(Enemy2Attack), typeof(Enemy2Move))]
public class Enemy2Detect : MonoBehaviour
{
    [Header("발사 설정")]
    public float detectionRange = 10f;
    public float shootInterval = 0.5f;
    public float preShootStopDuration = 1f;

    private Enemy2Attack shooter;
    private Enemy2Move mover;
    private Transform player;
    private bool isChasing = false;

    private TimerHandler timerHandler = new TimerHandler();

    private void Awake()
    {
        shooter = GetComponent<Enemy2Attack>();
        mover = GetComponent<Enemy2Move>();
        FindPlayer();
    }

    private void Update()
    {
        if (player == null) { FindPlayer(); return; }

        float dist = Vector2.Distance(transform.position, player.position);

        if (!isChasing && dist <= detectionRange)
        {
            isChasing = true;
            StartShooting();
        }
        else if (isChasing && dist > detectionRange)
        {
            isChasing = false;
            mover.StopMove = false;
            timerHandler.ClearAllTimers();
        }

        timerHandler.UpdateTimers(Time.deltaTime);
    }

    private void StartShooting()
    {
        if (!isChasing) return;

        mover.StopMove = true;
        timerHandler.AddTimer(preShootStopDuration, () =>
        {
            mover.StopMove = false;
            shooter.ShootAtPlayer();

            timerHandler.AddTimer(shootInterval, ShootRepeat, repeat: true);
        });
    }

    private void ShootRepeat()
    {
        if (!isChasing) return;

        mover.StopMove = true;
        timerHandler.AddTimer(preShootStopDuration, () =>
        {
            mover.StopMove = false;
            shooter.ShootAtPlayer();
        });
    }

    private void FindPlayer()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

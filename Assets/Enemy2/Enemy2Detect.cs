using UnityEngine;

[RequireComponent(typeof(Enemy2Attack), typeof(Enemy2Move), typeof(Animator))]
public class Enemy2Detect : MonoBehaviour
{
    [Header("설정")]
    public float detectionRange = 10f;       // 파란 Gizmo 범위
    public float redRangeFactor = 1.5f;      // 붉은 Gizmo 범위 = detectionRange / 1.5
    public float preShootStopDuration = 1f;  // 차징 시간
    public float shootCooldown = 1f;         // 발사 후 쿨타임

    private float shootCooldownTimer = 0f;   // 발사 후 쿨타임 타이머
    private bool firstShotDone = false;

    private Enemy2Attack shooter;
    private Enemy2Move mover;

    private float dist;
    private float stateTimer;

    private Transform player;

    public enum TopState { Idle, Detected }
    public enum SubState { None, Chase, Charging, Shoot }

    public TopState CurrentTopState => topState;
    public SubState CurrentSubState => subState;

    private TopState topState = TopState.Idle;
    private SubState subState = SubState.None;

    private void Awake()
    {
        shooter = GetComponent<Enemy2Attack>();
        mover = GetComponent<Enemy2Move>();
        FindPlayer();
    }

    public void Update()
    {
        if (player == null) { FindPlayer(); return; }
        dist = Vector2.Distance(transform.position, player.position);

        switch (topState)
        {
            case TopState.Idle:
                if (dist <= detectionRange)
                {
                    ChangeTopState(TopState.Detected, SubState.Chase);

                    if (!mover.FirstDetect)
                        mover.FirstDetect = true;
                }
                break;

            case TopState.Detected:
                UpdateDetected();
                if (dist > detectionRange)
                {
                    ChangeTopState(TopState.Idle, SubState.None);
                }
                break;
        }
    }
    public void UpdateDetected()
    {
        switch (subState)
        {

            case SubState.Chase:
                mover.StopMove = false;

                if (shootCooldownTimer > 0f)
                {
                    shootCooldownTimer -= Time.deltaTime;
                }
                else if (dist <= detectionRange / redRangeFactor)
                {
                    ChangeSubState(SubState.Charging);
                }
                break;

            case SubState.Charging:
                mover.StopMove = true;

                stateTimer -= Time.deltaTime;
                if (dist > detectionRange / redRangeFactor)
                {
                    ChangeSubState(SubState.Chase);
                }
                else if (stateTimer <= 0f)
                {
                    ChangeSubState(SubState.Shoot);
                }
                break;

            case SubState.Shoot:
                mover.StopMove = true;
                
                break;
        }
    }

    private void ChangeTopState(TopState newTop, SubState newSub)
    {
        if (topState == newTop && subState == newSub) return;

        topState = newTop;
        subState = newSub;

        if (newSub == SubState.Charging)
            stateTimer = preShootStopDuration;
    }

    private void ChangeSubState(SubState newSub)
    {
        if (subState == newSub) return;

        subState = newSub;

        if (newSub == SubState.Charging)
            stateTimer = preShootStopDuration;
        else if (newSub == SubState.Shoot)
            stateTimer = 0.45f;
    }

    public void OnShootEnd()
    {

        shootCooldownTimer = shootCooldown;

        ChangeSubState(SubState.Chase);
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange / redRangeFactor);
    }
}

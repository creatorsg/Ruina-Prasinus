using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Elite1State))]
[RequireComponent(typeof(Elite1Handler))]
[RequireComponent(typeof(Rigidbody2D))]
public class Elite1action : MonoBehaviour
{
    private Elite1State state;
    private Elite1Handler handler;
    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        state = GetComponent<Elite1State>();
        handler = GetComponent<Elite1Handler>();
        rb = GetComponent<Rigidbody2D>();

        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;

        state.OnStateChanged += HandleStateChange;
    }

    private void HandleStateChange(Elite1State.State newState)
    {
        switch (newState)
        {
            case Elite1State.State.Idle:
                StateIdle(); // 상태 전환 직후 멈춤
                break;
            case Elite1State.State.Attack:
                StartCoroutine(StateAttack()); // 코루틴 실행
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state.currentState)
        {
            case Elite1State.State.Chase:
                StateChase();
                break;
        }
    }

    private void StateIdle()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void StateChase()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * handler.moveSpeed, rb.linearVelocity.y);
    }

    private IEnumerator StateAttack()
    {
        if (player == null) yield break;

        // 공격 실행 (한 번만)
        if (Random.value < 0.5f)
            AttackDash();
        else
            AttackJump();

        // 1초 대기
        yield return new WaitForSeconds(3f);

        // 공격 후 이전 상태로 복귀
        state.ReturnToPreviousState();
    }

    private void AttackDash()
    {
        if (player == null) return;
        StartCoroutine(AttackDashRoutine());
    }

    private IEnumerator AttackDashRoutine()
    {
        // 0.5초 대기 (돌진 준비)
        yield return new WaitForSeconds(0.5f);

        if (player == null) yield break;

        // 돌진 (플레이어 방향으로 빠르게 이동)
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * handler.attackDashSpeed, rb.linearVelocity.y);
    }


    private void AttackJump()
    {
        if (player == null) return;
        StartCoroutine(AttackJumpRoutine());
    }

    private IEnumerator AttackJumpRoutine()
    {
        // 0.5초 대기 (점프 준비 모션 등)
        yield return new WaitForSeconds(0.5f);

        if (player == null) yield break;

        // 플레이어 방향 (x축 기준)
        Vector2 dir = (player.position - transform.position).normalized;

        // 앞으로 + 위로 점프
        rb.linearVelocity = new Vector2(dir.x * handler.moveSpeed * 2f, handler.jumpForce);
    }

}

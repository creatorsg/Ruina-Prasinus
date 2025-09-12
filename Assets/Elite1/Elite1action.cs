using UnityEngine;

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
                StateAttack(); // Attack 상태 진입 시 한 번만 실행
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state.currentState) // ✅ 여기서 currentState 사용
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

    private void StateAttack()
    {
        if (player == null) return;

        if (Random.value < 0.5f)
            AttackDash();
        else
            AttackJump();
    }

    private void AttackDash()
    {
        // 돌진
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * handler.attackDashSpeed, rb.linearVelocity.y);
    }

    private void AttackJump()
    {
        // 점프
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, handler.jumpForce);
    }

}

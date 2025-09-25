using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Elite1State))]
[RequireComponent(typeof(Elite1Handler))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EliteAnime))]
public class Elite1action : MonoBehaviour
{
    private Elite1State state;
    private Elite1Handler handler;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;

    public bool IsGrounded() => isGrounded;

    public event System.Action OnDashEnded;
    public enum AttackType { None, Dash, Jump }

    public event System.Action<bool, AttackType> OnAttackStateChanged;

    private bool isAttacking = false;
    public bool IsAttacking => isAttacking;

    private AttackType currentAttack = AttackType.None;
    public AttackType CurrentAttack => currentAttack;

    [Header("Ground 체크")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;
    private bool isGrounded = false;
    private bool wasGrounded = false;

    private void Awake()
    {
        state = GetComponent<Elite1State>();
        handler = GetComponent<Elite1Handler>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) player = go.transform;

        EliteAnime eliteAnime = GetComponent<EliteAnime>();
        if (eliteAnime != null)
        {
            eliteAnime.OnLanded += HandleLanded;
        }

        state.OnStateChanged += HandleStateChange;
    }

    private void Update()
    {
        // 공격 중이 아닐 때만 플레이어 바라보기
        if (!isAttacking && player != null)
            LookAtPlayer();

        // 점프 후 하강 여부 체크
        animator.SetBool("isFalling", rb.linearVelocity.y < -0.1f);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        // wasGrounded 업데이트만 수행
        wasGrounded = isGrounded;
    }


    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleStateChange(Elite1State.State newState)
    {
        if (newState == Elite1State.State.Attack)
        {
            StartCoroutine(StateAttack());
        }
    }

    private IEnumerator StateAttack()
    {
        if (player == null) yield break;

        // 랜덤 공격 선택
        if (Random.value < 0.5f)
        {
            SetAttackState(true, AttackType.Dash);
            AttackDash();
        }
        else
        {
            SetAttackState(true, AttackType.Jump);
            AttackJump();
        }

        // 공격 지속 시간
        yield return new WaitForSeconds(3f);

        SetAttackState(false, AttackType.None);
        state.ReturnToPreviousState();
    }

    #region AttackDash
    private void AttackDash()
    {
        if (player == null) return;
        StartCoroutine(AttackDashRoutine());
    }

    private IEnumerator AttackDashRoutine()
    {
        yield return new WaitForSeconds(0.5f); // 준비 시간

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * handler.attackDashSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(1.0f); // 돌진 유지 시간

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        OnDashEnded?.Invoke();
        // Dash 종료 신호
        SetAttackState(false, AttackType.Dash);
    }

    #endregion


    #region AttackJump
    private void AttackJump()
    {
        if (player == null) return;
        StartCoroutine(AttackJumpRoutine());
    }

    private IEnumerator AttackJumpRoutine()
    {
        yield return new WaitForSeconds(0.5f); // 점프 준비

        if (player == null) yield break;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * handler.moveSpeed * 2f, handler.jumpForce);

        // Jump 상태 시작 신호만 보냄
        SetAttackState(true, AttackType.Jump);

        while (!isGrounded)
        {
            yield return null; // 다음 프레임까지 대기
        }

        // 착지 시 X축 속도만 0으로
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
    #endregion


    private void LookAtPlayer()
    {
        if (player == null) return;

        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void SetAttackState(bool attacking, AttackType type)
    {
        isAttacking = attacking;
        currentAttack = type;
        OnAttackStateChanged?.Invoke(isAttacking, currentAttack);
    }

    private void HandleLanded()
    {
        // 착지 시 X축 속도를 0으로 설정
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // Jump 공격이 끝났다면 공격 상태도 해제
        if (currentAttack == AttackType.Jump)
        {
            SetAttackState(false, AttackType.None);
            state.ReturnToPreviousState();
        }
    }


}

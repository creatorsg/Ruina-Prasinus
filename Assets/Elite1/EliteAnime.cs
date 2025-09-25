using UnityEngine;
using System.Collections;
using static Elite1action;

[RequireComponent(typeof(Rigidbody2D))]
public class EliteAnime : MonoBehaviour
{
    [SerializeField] private Elite1action elite1Action;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;


    public event System.Action OnLanded;
    private enum JumpAnimState
    {
        None,
        Jumping,   // 상승 중
        Falling,   // 하강 중
        Landing    // 착지 중
    }

    private JumpAnimState jumpState = JumpAnimState.None;

    private void Awake()
    {
        if (elite1Action != null)
            elite1Action.OnAttackStateChanged += HandleAttackState;
    }

    private void Update()
    {
        if (jumpState != JumpAnimState.None)
        {
            UpdateJumpState();
        }
    }

    private void HandleAttackState(bool isAttacking, AttackType attackType)
    {
        if (!isAttacking)
        {
            if (jumpState == JumpAnimState.None)
                animator.Play("Idle", 0, 0f);
            return;
        }

        switch (attackType)
        {
            case AttackType.Dash:
                animator.Play("Running", 0, 0f);
                // 대쉬 상태가 끝나면 Idle로 돌아가도록 이벤트로 처리
                elite1Action.OnDashEnded += HandleDashEnded;
                break;

            case AttackType.Jump:
                if (jumpState == JumpAnimState.None)
                    StartJumpSequence();
                break;
        }
    }

    private void HandleDashEnded()
    {
        // 대쉬 종료 이벤트 받으면 Idle 재생
        animator.Play("Idle", 0, 0f);
        // 이벤트 해제 (중복 호출 방지)
        elite1Action.OnDashEnded -= HandleDashEnded;
    }


    // ---------------------------
    // 점프 애니메이션 관리
    // ---------------------------
    private void StartJumpSequence()
    {
        if (jumpState != JumpAnimState.None) return; // 이미 점프 중이면 무시

        animator.Play("Jump", 0, 0f);
        jumpState = JumpAnimState.Jumping;
    }

    private void UpdateJumpState()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        switch (jumpState)
        {
            case JumpAnimState.Jumping:
                // Jump 애니메이션이 최소 10% 재생된 후 속도 음수이면 Falling 전환
                if (stateInfo.IsName("Jump") && stateInfo.normalizedTime >= 0.1f && rb.linearVelocity.y < 0)
                {
                    animator.Play("JumpToDown", 0, 0f);
                    jumpState = JumpAnimState.Falling;
                }
                break;

            case JumpAnimState.Falling:
                // 기존 속도 기반 애니메이션 전환 유지
                if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
                {
                    animator.Play("Land", 0, 0f);
                    jumpState = JumpAnimState.Landing;
                }
                break;


            case JumpAnimState.Landing:
                // Land 애니메이션 끝나면 Idle로 복귀
                if (stateInfo.IsName("Land") && stateInfo.normalizedTime >= 1f)
                {
                    
                    animator.Play("Idle", 0, 0f);
                    jumpState = JumpAnimState.None;
                }
                break;
        }
    }



    private IEnumerator EndDashAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
        {
            animator.Play("Idle", 0, 0f);
        }
    }
}

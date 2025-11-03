using UnityEngine;

public class AnimationAttack : MonoBehaviour
{
    [SerializeField] private JangpungController jangpungController;
    [SerializeField] private AnimationTotal animationTotal;

    private bool isUp;
    private bool isDown;
    private bool isWalk;
    private bool isGround;

    // ✅ 공격 중 여부 (Notifier 기능 통합)
    public bool IsAttacking { get; private set; }

    public int AttackNum { get; private set; } = 0;

    private Animator animator;

    private enum PlayerState { Ground, Air }

    void Awake()
    {
        jangpungController.OnAttack += Attack;
        jangpungController.OnLookUp += HandleLookUp;
        jangpungController.OnLieDown += HandleLieDown;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animationTotal != null)
        {
            isGround = animationTotal.isGround;
            isWalk = animationTotal.isWalk;
        }

        // 예: 공격 중일 때 다른 입력 무시 가능
        // if (IsAttacking) Debug.Log("공격 중...");
    }

    private void Attack()
    {
        // ✅ 공격 중일 땐 중복 공격 방지
        if (IsAttacking)
            return;

        // Ground
        if (isGround)
        {
            if (isWalk)
            {
                if (AttackNum == 0)
                {
                    animator.Play("RunningAttack1");
                    AttackNum = 1;
                }
                else
                {
                    animator.Play("RunningAttack2");
                    AttackNum = 0;
                }
            }
            else
            {
                if (AttackNum == 0)
                {
                    animator.Play("GroundAttack1");
                    AttackNum = 1;
                }
                else
                {
                    animator.Play("GroundAttack2");
                    AttackNum = 0;
                }
            }
        }
        else
        {
            // Air
            if (isUp)
            {
                animator.Play("TopAttack");
            }
            else if (isDown)
            {
                animator.Play("UnderAttack");
            }
            else
            {
                animator.Play("JumpAttack");
            }
        }
    }

    private void HandleLookUp(bool up) => isUp = up;
    private void HandleLieDown(bool down) => isDown = down;

    // ✅ 애니메이션 이벤트에서 호출할 함수
    // 공격 애니메이션 첫 프레임에서 실행
    public void AttackStart()
    {
        IsAttacking = true;
        // Debug.Log("공격 시작");
    }

    // 공격 애니메이션 마지막 프레임에서 실행
    public void AttackEnd()
    {
        IsAttacking = false;
        // Debug.Log("공격 종료");
    }
}

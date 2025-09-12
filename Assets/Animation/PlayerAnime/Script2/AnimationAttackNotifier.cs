using UnityEngine;

public class AnimationAttackNotifier : MonoBehaviour
{
    private Animator animator;

    // 공격 애니메이션 재생 여부
    public bool IsAttacking { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 애니메이션 이벤트에서 호출 (첫 프레임)
    public void AttackStart()
    {
        IsAttacking = true; // 공격 시작
    }

    // 애니메이션 이벤트에서 호출 (마지막 프레임)
    public void AttackEnd()
    {
        IsAttacking = false; // 공격 종료
    }
}

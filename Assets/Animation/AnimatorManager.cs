using UnityEngine;

namespace Player
{

    public class AnimatorManager : MonoBehaviour
    {
        private Animator animator;

        public int AttackNum { get; private set; } = 0;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // 이동 상태 설정
        public void SetMoveState(bool isMove)
        {
            animator.SetBool("isMove", isMove);
            Debug.Log($"SetMoveState: {isMove}");   
        }

        // 점프 상태 설정
        public void SetJumpTrigger()
        {
            animator.SetTrigger("Jump");
        }

        // 착지 상태
        public void SetGroundState(bool isGround)
        {
            animator.SetBool("isGround", isGround);
        }

        public void SetAttackTrigger()
        {
            animator.SetTrigger("Attack");
            AttackNum++;
            AttackNum = AttackNum % 2;
            animator.SetInteger("Attack_NumCheck", AttackNum);  
        }
    }
}

using UnityEngine;

namespace Player
{

    public class AnimatorManager : MonoBehaviour
    {
        private Animator animator;

        public int AttackNum { get; private set; } = 0;
        public int MoveNum { get; private set; } = 0;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

            // 이동 상태 설정
        public void SetMoveBool(bool isMove)
        {
            animator.SetBool("isMove", isMove);

        }



        public void SetUpBool(bool Up)
        {
            animator.SetBool("Up", Up);
        }

        public void SetDownBool(bool Down)
        {
            animator.SetBool("Down", Down);
        }


        // 착지 상태
        public void SetGroundBool(bool isGround)
        {
            animator.SetBool("isGround", isGround);
            
        }

        public void SetAttackTrigger()
        {
            Debug.Log("Attack Trigger Called");
            animator.SetTrigger("Attack");
            AttackNum++;
            AttackNum = AttackNum % 2;
            animator.SetInteger("Attack_NumCheck", AttackNum);  
        }

        
    }
}

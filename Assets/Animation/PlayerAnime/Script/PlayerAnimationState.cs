using UnityEngine;

namespace Player
{
    public class PlayerAnimationState : MonoBehaviour
    {
        [SerializeField] private AnimatorManager animatorManager;
        [SerializeField] private JangpungController jangpungController;
        [SerializeField] private MoveStatusHandler moveStatusHandler;
        private Animator animator;

        
        private bool isGround;
        private bool isSitting;

        private enum PlayerState { Ground, Air }
        private enum SubState { Move, Stop }

        private PlayerState currentState;
        private SubState currentSubState;

        void Awake()
        {
            animator = GetComponent<Animator>();

            // 이벤트 구독
            jangpungController.OnLookUp += LookUp;
            jangpungController.OnLieDown += SitDown;
            moveStatusHandler.OnGroundStateChanged += SetGroundBool;

            // 초기 상태
            currentState = PlayerState.Ground;
            currentSubState = SubState.Stop;
        }

        private void Update()
        {
            bool isMoving = false; // TODO: 실제 이동 로직과 연결
            currentSubState = isMoving ? SubState.Move : SubState.Stop;

            // 👉 외부에서 SetGroundBool 로 전달된 값 사용
            currentState = isGround ? PlayerState.Ground : PlayerState.Air;
        }

        private void LookUp(bool up)
        {
            

            if (!isGround) return;

            if (currentSubState != SubState.Stop) return;

            if (up)
            {
                animator.Play("UpSee", 0);
            }
            else
                animator.Play("Stand");
        }

        private void SitDown(bool down)
        {
            
        }

        // 더 이상 OnSitIdleEnd / OnStandingEnd에서 Play() 호출 필요 없음
        public void OnSitIdleEnd()
        {
            isSitting = true;
        }

        public void OnStandingEnd()
        {
            // 필요시 추가 로직만
        }



        public void SetGroundBool(bool ground)
        {
            isGround = ground;
        }

        public void SetMoveBool(bool isMove)
        {
            animator.SetBool("isMove", isMove);
        }
    }
}

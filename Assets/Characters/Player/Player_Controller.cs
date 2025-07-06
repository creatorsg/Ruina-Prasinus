using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Player
{
    public class Player_Controller : MonoBehaviour
    {
        // 모델(상태)과 뷰는 코드로 생성/할당
        private Player_Model model = new Player_Model();
        private Player_View view;

        [Header("Data (ScriptableObject)")]
        [SerializeField] private PlayerData playerData;

        [Header("— 걷기 (Walking) —")]
        private float moveInput = 0f;
        private float walkAccelTimer = 0f;
        private int facingDirection = 1;

        [Header("— 대시 (Dash) —")]
        private float dashTimer = 0f;
        private int dashDirection = 0;
        private float cooltime = 0f;

        [Header("— 점프 (Jump) —")]
        private bool isJumping = false;

        [Header("- 키 입력 (Press) -")]
        bool dashKeyHeld = false;
        void Awake()
        {
            if (playerData == null)
                Debug.LogError("PlayerData 에셋이 할당되지 않았습니다!", this);

            view = GetComponent<Player_View>();
            if (view == null)
                Debug.LogError("Player_View 컴포넌트가 없습니다!", this);
        }

        void Start()
        {
            playerData.hp = 500f;    
        }


        void Update()
        {
            var m = model;

            if (model.isGrounded && Input.GetButtonDown("Jump"))
            {
                isJumping = true;
            }

            // 대시 입력
            dashKeyHeld = Input.GetKey(KeyCode.LeftShift);

            if (!model.isDashing && dashKeyHeld && Input.GetAxisRaw("Horizontal") != 0 && model.isGrounded && m.canDash != false)
            {
                model.isDashing = true;
                dashTimer = 0f;
                dashDirection = Input.GetAxisRaw("Horizontal") > 0 ? 1 : -1;
            }

            handleDashCoolDown();

            if (m.isHit || m.inCinematic || m.inGearSetting || m.inGearAction || m.inUIControl)
                return;


            HandleInput();
        }

        void FixedUpdate()
        {
            HandleWalk();
            HandleDash();
            HandleJump();
        }

        void HandleInput()
        {
            dashKeyHeld = Input.GetKey(KeyCode.LeftShift);
        }

        void HandleWalk()
        {
            var m = model;

            moveInput = Input.GetAxisRaw("Horizontal");

            if (moveInput != 0)
            {
                facingDirection = moveInput > 0 ? 1 : -1;
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * facingDirection;
                transform.localScale = scale;

                walkAccelTimer += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(walkAccelTimer / playerData.walkAccelTime);
                playerData.currentWalkSpeed = Mathf.Lerp(0f, playerData.walkMaxSpeed, t);

                view.UpdateWalk(moveInput * playerData.currentWalkSpeed);
            }
            else
            {
                walkAccelTimer = 0f;
                playerData.currentWalkSpeed = 0f;
                view.UpdateWalk(moveInput * playerData.currentWalkSpeed);
            }
        }

        void HandleDash()
        {
            var m = model;

            // 대시 시작
            dashTimer += Time.fixedDeltaTime;

            if (dashTimer < playerData.dashAccelTime)
            {
                float t = dashTimer / playerData.dashAccelTime;
                playerData.currentDashSpeed = Mathf.Lerp(0f, playerData.dashMaxSpeed, t);
            }
            else if (dashTimer < playerData.dashDuration)
            {
                playerData.currentDashSpeed = playerData.dashMaxSpeed;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    model.isDashing = false;
                    playerData.currentDashSpeed = 0f;
                    return;
                }
            }
            else
            {
                if (model.isGrounded)
                {
                    model.isDashing = false;
                    playerData.currentDashSpeed = 0f;
                    return;
                }

                if (!model.isGrounded)
                {
                    model.isDashing = false;
                    playerData.currentDashSpeed = playerData.dashMaxSpeed;
                    return;
                }

            }
            view.UpdateDash(dashDirection * playerData.currentDashSpeed);
            m.canDash = false;
        }

        void handleDashCoolDown()
        {
            var m = model;

            if (cooltime <= 0f)
            {
                cooltime = playerData.dashCooldown;
                m.canDash = true;
            }

            if (m.canDash == false)
            {
                cooltime -= Time.fixedDeltaTime;
            }
        }

        void HandleJump()
        {
            if (!isJumping)
                return;


            if (isJumping && model.isGrounded)
            {
                view.JumpImpulse(playerData.jumpSpeed);

                view.ContinueJump(playerData.jumpSpeed);
            }

            isJumping = false;
            model.isGrounded = false;
        }

        void HandleEnvincible()
        {

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                model.isGrounded = true;
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                model.isGrounded = false;
            }
        }
    }
}
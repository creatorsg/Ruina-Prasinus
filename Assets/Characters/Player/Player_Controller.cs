using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Player
{
    public class Player_Controller : MonoBehaviour
    {
        public static Player_Controller Instance { get; private set; }

        private Player_Model model = new Player_Model();
        private Player_View view;

        [Header("Data (ScriptableObject)")]
        [SerializeField] private PlayerData playerData;

        [Header("— 걷기 (Walking) —")]
        private int facingDirection = 1;
        private float walkAccelTimer = 0f;

        [Header("— 대시 (Dash) —")]
        private float dashTimer = 0f;
        private int dashDirection = 0;
        private float cooltime = 0f;

        [Header("— 점프 (Jump) —")]
        private bool isJumping = false;

        [Header("- 키 입력 (Press) -")]
        bool dashKeyHeld = false;

        private float previousHp;
        void Awake()
        {
            view = GetComponent<Player_View>();
            if (view == null)
                Debug.LogError("Player_View 컴포넌트가 없습니다!", this);
        }

        void Start()
        {
            playerData.hp = 500f;
            previousHp = playerData.hp;
        }


        void Update()
        {
            var m = model;

            if (model.isGrounded && InputManager.GetKeyDown("Jump"))
            {
                isJumping = true;
            }


            if (!m.isDashing && InputManager.GetKeyDown("Dash") && m.isGrounded && m.canDash)
            {
                m.isDashing = true;
                dashTimer = 0f;
                dashDirection = facingDirection;  
            }

            handleDashCoolDown();

            if (playerData.hp < previousHp && !model.isHit && !model.isInvincible)
            {
                OnHit();
            }

            if (m.isHit)
            {
                transform.position += new Vector3(0, 0.005f, 0); // 밀림 연출
                playerData.freezeTimer += Time.deltaTime;

                if (playerData.freezeTimer >= playerData.freezeDuration)
                {
                    m.isHit = false;
                    playerData.freezeTimer = 0f;

                    // 무적 시작
                    m.isInvincible = true;
                    playerData.invincibleTimer = 0f;
                }
            }

            if (m.isInvincible)
            {
                playerData.invincibleTimer += Time.deltaTime;
                if (playerData.invincibleTimer >= playerData.invincibleDuration)
                {
                    m.isInvincible = false;
                    playerData.invincibleTimer = 0f;
                    view.StopBlink(); // 무적 해제 시 깜빡임 중지
                }
            }

            if (m.isHit || m.inCinematic || m.inGearSetting || m.inGearAction || m.inUIControl)
                return;

            previousHp = playerData.hp;

            HandleInput();
        }

        void FixedUpdate()
        {
            var m = model;

            if (m.isHit || m.inCinematic || m.inGearSetting || m.inGearAction || m.inUIControl)
                return;

            HandleWalk();
            HandleDash();
            HandleJump();
        }

        void HandleInput()
        {
            dashKeyHeld = InputManager.GetKey("Dash");
        }

        private void HandleWalk()
        {
            float moveInput = 0f;
            if (InputManager.GetKey("MoveLeft"))  moveInput -= 1f;
            if (InputManager.GetKey("MoveRight")) moveInput += 1f;

            if (moveInput != 0f)
            {
                int dir = moveInput > 0 ? 1 : -1;
                facingDirection = dir;
                var s = transform.localScale;
                s.x = Mathf.Abs(s.x) * dir;
                transform.localScale = s;

                walkAccelTimer += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(walkAccelTimer / playerData.walkAccelTime);
                playerData.currentWalkSpeed = Mathf.Lerp(0f, playerData.walkMaxSpeed, t);
            }
            else
            {
                walkAccelTimer = 0f;
                playerData.currentWalkSpeed = 0f;
            }

            view.UpdateWalk(moveInput, playerData.currentWalkSpeed);
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

        public void OnHit()
        {
            if (!model.isHit)
            {
                model.isHit = true;
                playerData.freezeTimer = 0f;
                view.StartBlink(); 
            }
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
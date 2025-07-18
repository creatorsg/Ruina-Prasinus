/*
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.VisualScripting;

namespace Player
{
    public class Player_Controller : MonoBehaviour
    {
        private Player_Model model = new Player_Model();
        Rigidbody2D rb = null;
        private Player_View view;

        [Header("Data (ScriptableObject)")]
        [SerializeField] private PlayerState state;

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
            rb = GetComponent<Rigidbody2D>();
            
        }

        void Start()
        {
            state.hp = 500f;
            previousHp = state.hp;
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

            if (state.hp < previousHp && !model.isHit && !model.isInvincible)
            {
                OnHit();
            }

            if (m.isHit)
            {
                transform.position += new Vector3(0, 0.005f, 0); // 밀림 연출
                state.freezeTimer += Time.deltaTime;

                if (state.freezeTimer >= state.freezeDuration)
                {
                    m.isHit = false;
                    state.freezeTimer = 0f;

                    // 무적 시작
                    m.isInvincible = true;
                    state.invincibleTimer = 0f;
                }
            }

            if (m.isInvincible)
            {
                state.invincibleTimer += Time.deltaTime;
                if (state.invincibleTimer >= state.invincibleDuration)
                {
                    m.isInvincible = false;
                    state.invincibleTimer = 0f;
                    view.StopBlink(); // 무적 해제 시 깜빡임 중지
                }
            }

            if (m.isHit || m.inCinematic || m.inGearSetting || m.inGearAction || m.inUIControl)
                return;

            previousHp = state.hp;

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
                float t = Mathf.Clamp01(walkAccelTimer / state.walkAccelTime);
                state.currentWalkSpeed = Mathf.Lerp(0f, state.walkMaxSpeed, t);
            }
            else
            {
                walkAccelTimer = 0f;
                state.currentWalkSpeed = 0f;
            }

            view.UpdateWalk(moveInput, state.currentWalkSpeed);
        }

        void HandleDash()
        {
            var m = model;

            // 대시 시작
            dashTimer += Time.fixedDeltaTime;

            if (dashTimer < state.dashAccelTime)
            {
                float t = dashTimer / state.dashAccelTime;
                state.currentDashSpeed = Mathf.Lerp(0f, state.dashMaxSpeed, t);
            }
            else if (dashTimer < state.dashDuration)
            {
                state.currentDashSpeed = state.dashMaxSpeed;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    model.isDashing = false;
                    state.currentDashSpeed = 0f;
                    return;
                }
            }
            else
            {
                if (model.isGrounded)
                {
                    model.isDashing = false;
                    state.currentDashSpeed = 0f;
                    return;
                }
            }
            view.UpdateDash(dashDirection * state.currentDashSpeed);
            m.canDash = false;
        }

        void handleDashCoolDown()
        {
            var m = model;

            if (cooltime <= 0f)
            {
                cooltime = state.dashCooldown;
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
                view.JumpImpulse(state.jumpSpeed);

                view.ContinueJump(state.jumpSpeed);
            }

            isJumping = false;
            model.isGrounded = false;
        }

        public void OnHit()
        {
            if (!model.isHit)
            {
                model.isHit = true;
                state.freezeTimer = 0f;
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
*/
using UnityEngine;
using System.Collections;

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
    bool dashKeyHeld = false;

    [Header("— 점프 (Jump) —")]
    private float jumpTimer = 0f;

    void Awake()
    {
        // ScriptableObject 참조와 뷰 컴포넌트 체크
        if (playerData == null)
            Debug.LogError("PlayerData 에셋이 할당되지 않았습니다!", this);

        view = GetComponent<Player_View>();
        if (view == null)
            Debug.LogError("Player_View 컴포넌트가 없습니다!", this);
    }

    void Update()
    {
        var m = model;

        if (model.isGrounded && Input.GetButtonDown("Jump"))
        {
            model.isJumping = true;
        }

        // 대시 입력
        dashKeyHeld = Input.GetKey(KeyCode.LeftShift);

        if (!model.isDashing && dashKeyHeld && Input.GetAxisRaw("Horizontal") != 0)
        {
            model.isDashing = true;
            dashTimer = 0f;
            dashDirection = Input.GetAxisRaw("Horizontal") > 0 ? 1 : -1;
        }

        if (m.isHit || m.inCinematic || m.inGearSetting || m.inGearAction || m.inUIControl)
            return;


        HandleInput();
    }

    void FixedUpdate()
    {
        HandleWalk();
        HandleDash();
        HandleJump();
        HandleFall();
    }

    void HandleInput()
    {
        if (model.isGrounded && !model.isJumping && !model.isFalling && Input.GetKeyDown(KeyCode.Space))
        {
            StartJump();
        }

        else if (model.isDashing && Input.GetKeyDown(KeyCode.Space) && model.canDash)
        {
            StartJump();
        }

        dashKeyHeld = Input.GetKey(KeyCode.LeftShift);
    }
    void HandleWalk()
    {
        var m = model;

        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            // 방향 전환
            facingDirection = moveInput > 0 ? 1 : -1;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * facingDirection;
            transform.localScale = scale;

            // 가속
            walkAccelTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(walkAccelTimer / playerData.walkAccelTime);
            playerData.currentWalkSpeed = Mathf.Lerp(0f, playerData.walkMaxSpeed, t);

            view.UpdateWalk(moveInput * playerData.currentWalkSpeed);
        }
        else
        {
            // 멈춤
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
            model.isDashing = false;
            playerData.currentDashSpeed = 0f;
            return;
        }
        view.UpdateDash(dashDirection * playerData.currentDashSpeed);
    }

    IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(playerData.dashCooldown);
        model.canDash = true;
    }

    void HandleJump()
    {
        if (!model.isJumping) return;

        jumpTimer += Time.fixedDeltaTime;

        // 스페이스 키를 누르고 있는 동안, 그리고 최대 홀드 시간 이하라면
        if (Input.GetKey(KeyCode.Space) && jumpTimer <= playerData.jumpHoldTime)
        {
            view.ContinueJump(playerData.jumpSpeed);
        }
        else
        {
            // 홀드 타임 초과 또는 키 놓으면 바로 낙하 모드로
            model.isJumping = false;
            model.isFalling = true;
        }
    }

    void StartJump()
    {
        model.isJumping = true;
        model.isGrounded = false;
        model.isFalling = false;
        jumpTimer = 0f;
        view.JumpImpulse(playerData.jumpSpeed);
    }

    void HandleFall()
    {
        var m = model;
        if (!m.isGrounded && !m.isJumping)
            m.isFalling = true;
        else if (m.isGrounded)
        {
            m.isFalling = false;
            m.isJumping = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // 바닥 태그 + 노말 검사
        if (col.gameObject.CompareTag("Ground") && col.contacts[0].normal.y > 0.5f)
            model.isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            model.isGrounded = false;
    }
}

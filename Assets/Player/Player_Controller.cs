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

    [Header("— 점프 (Jump) —")]
    private float jumpTimer = 0f;
    private bool isGrounded = false;
    private bool isJumping = false;

    [Header("- 키 입력 (Press) -")]
    bool dashKeyHeld = false;
    bool jumpKeyHeld = false;
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

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
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
            model.isDashing = false;
            playerData.currentDashSpeed = 0f;
            return;
        }
        view.UpdateDash(dashDirection * playerData.currentDashSpeed);
    }

    void HandleJump()
    {
        if (!isJumping)
            return;


        if (isJumping && isGrounded)
        {
            view.JumpImpulse(playerData.jumpSpeed);

            view.ContinueJump(playerData.jumpSpeed);
        }

        isJumping = false;
        isGrounded = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

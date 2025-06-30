using UnityEngine;
using System.Collections;

public class Player_Controller : MonoBehaviour
{
    // 상태와 스탯을 분리한 두 클래스
    private Player_Model model = new Player_Model();
    private PlayerData playerData;
    private Player_View view;

    private float walkAccelTimer = 0f;
    private float dashAccelTimer = 0f;
    private float dashTimer = 0f;
    private float jumpTimer = 0f;

    void Awake()
    {
        // 같은 게임오브젝트에 붙어 있는 컴포넌트 자동 할당
        playerData = GetComponent<PlayerData>();
        view = GetComponent<Player_View>();

        // 없으면 에디터 콘솔에 오류 출력
        if (playerData == null) Debug.LogError(" PlayerData 컴포넌트가 없습니다!", this);
        if (view == null) Debug.LogError(" Player_View 컴포넌트가 없습니다!", this);
    }

    void Update()
    {
        var m = model;
        // 공통 동작 불가 조건
        if (m.isHit || m.inCinematic || m.inGearSetting || m.inGearAction || m.inUIControl)
            return;

        HandleWalk();
        HandleDash();
        HandleJump();
        HandleFall();
    }

    void HandleWalk()
    {
        var m = model;
        if (m.isDashing || m.isJumping || m.isFalling) return;

        float input = Input.GetAxisRaw("Horizontal");
        if (input != 0f)
        {
            m.isWalking = true;
            walkAccelTimer += Time.deltaTime;
            playerData.currentWalkSpeed = Mathf.Lerp(
                0f, playerData.walkMaxSpeed, walkAccelTimer / playerData.walkAccelTime
            );
            view.UpdateWalk(playerData.currentWalkSpeed, (int)Mathf.Sign(input));
        }
        else
        {
            m.isWalking = false;
            walkAccelTimer = 0f;
            playerData.currentWalkSpeed = 0f;
            view.UpdateWalk(0f, 1);
        }
    }

    void HandleDash()
    {
        var m = model;
        // 대시 시작
        if (Input.GetKeyDown(KeyCode.LeftShift) && m.canDash && !m.isDashing)
        {
            m.isDashing = true;
            m.canDash = false;
            dashTimer = 0f;
            dashAccelTimer = 0f;
        }

        if (m.isDashing)
        {
            dashTimer += Time.deltaTime;
            dashAccelTimer += Time.deltaTime;
            float addedSpeed = Mathf.Lerp(
                0f, playerData.dashMaxSpeed, dashAccelTimer / playerData.dashAccelTime
            );
            playerData.currentDashSpeed = addedSpeed;

            int dir = (int)Mathf.Sign(transform.localScale.x);
            view.UpdateDash(addedSpeed, dir);

            if (dashTimer >= playerData.dashDuration || Input.GetKeyUp(KeyCode.LeftShift))
            {
                m.isDashing = false;
                dashTimer = 0f;
                playerData.currentDashSpeed = 0f;
                StartCoroutine(DashCooldownRoutine());
            }
        }
    }

    IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(playerData.dashCooldown);
        model.canDash = true;
    }

    void HandleJump()
    {
        var m = model;

        // 대시 중 점프 (관성 유지)
        if (m.isDashing && Input.GetKeyDown(KeyCode.Space))
        {
            StartJump();
            return;
        }

        // 땅에 있을 때만 일반 점프
        if (!m.isGrounded || m.isJumping || m.isFalling) return;

        if (Input.GetKeyDown(KeyCode.Space))
            StartJump();

        if (m.isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (Input.GetKey(KeyCode.Space) && jumpTimer <= playerData.jumpHoldTime)
            {
                view.Jump(playerData.jumpSpeed);
            }
            else
            {
                m.isJumping = false;
                m.isFalling = true;
            }
        }
    }

    void StartJump()
    {
        model.isJumping = true;
        model.isGrounded = false;
        model.isFalling = false;
        jumpTimer = 0f;
        view.Jump(playerData.jumpSpeed);
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
        // 땅 태그만 땅으로 인식
        if (col.gameObject.CompareTag("Ground") && col.contacts[0].normal.y > 0.5f)
            model.isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            model.isGrounded = false;
    }
}

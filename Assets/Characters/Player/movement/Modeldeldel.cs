using UnityEngine;

public class Modeldeldel
{
    public float CurrentSpeed { get; private set; }
    public float CurrentDashSpeed { get; private set; }
    public int FacingDirection { get; private set; } = 1;
    public bool isDashing { get; private set; }

    public float moveSpeed => CurrentSpeed + CurrentDashSpeed;
    public bool CanDash;

    float walkTimer, dashTimer, dashCooldownTimer, dashDuration = 3f;
    int  reaminspeed;
    bool dashHeld;


    public void Update(float moveInput, bool dashPressed, bool dashIsHeld, PlayerState state, float dt, bool isGrounded)
    {
        if (FacingDirection == 0) FacingDirection = 1;

        CanDash = dashCooldownTimer >= state.dashCooldownTime && !isDashing;
        dashCooldownTimer = Mathf.Min(dashCooldownTimer + dt, state.dashCooldownTime);

        reaminspeed = moveInput > 0f ? 1
                          : moveInput < 0f ? -1
                          : reaminspeed;
        dashHeld = dashIsHeld;

        if ( dashPressed && CanDash && isGrounded)
        {
            isDashing = true;
            dashTimer = 0f;
            dashCooldownTimer = 0f;
            FacingDirection = reaminspeed;
        }

        if (isDashing)
            UpdateDash(state, dt, isGrounded);
        else
            UpdateWalk(moveInput, state, dt);
    }

    void UpdateWalk(float input, PlayerState state, float dt)
    {
        CurrentDashSpeed = 0f;

        if (Mathf.Abs(input) > Mathf.Epsilon)
        {
            FacingDirection = input > 0f ? 1 : -1;
            walkTimer += dt;
            float t = Mathf.Clamp01(walkTimer / state.walkAccelTime);
            CurrentSpeed = Mathf.Lerp(0f, state.walkMaxSpeed, t);
        }
        else
        {
            walkTimer = 0f;
            CurrentSpeed = 0f;
        }
    }

    void UpdateDash(PlayerState state, float dt, bool isGround)
    {
        dashTimer += dt;

        if (dashTimer < state.dashAccelTime)
        {
            float t = Mathf.Clamp01(dashTimer / state.dashAccelTime);
            CurrentDashSpeed = Mathf.Lerp(CurrentSpeed, state.dashMaxSpeed, t);
            return;
        }
        if (dashTimer < dashDuration)
        {
            CurrentDashSpeed = state.dashMaxSpeed;
            if (!dashHeld)
            {
                EndDash(state);
            }
            return;
        }

        if (isGround)
        {
            EndDash(state);
        }
        else
        {
            CurrentDashSpeed = state.dashMaxSpeed;
            if (!dashHeld)
            {
                EndDash(state);
            }
            return;
        }
    }

    void EndDash(PlayerState state)
    {
        isDashing = false;
        CurrentDashSpeed = 0f;

        if (reaminspeed != 0)
        {
            walkTimer = state.walkAccelTime;
            CurrentSpeed = state.walkMaxSpeed;
            FacingDirection = reaminspeed;
        }
        else
        {
            walkTimer = 0f;
            CurrentSpeed = 0f;
        }
    }
}

using UnityEngine;

public class JumpModel
{
    [SerializeField] private PlayerStatus status;
    [SerializeField] private float jumpDuration = 0.25f;
    [SerializeField] private float gravityAccelTime = 0.25f;
    
    private float jumpPower = 20f;         
    private float maxFallSpeed = 25f;      
    public bool isJumping { get; private set; }
    public bool isFalling { get; private set; }
    public float CurrentVerticalSpeed { get; private set; }

    private float fallTimer;
    private float jumpTimer;

    public void Update(bool jumpHeld, bool isGrounded, float dt)
    {

        if (isGrounded)
        {
            Reset();
            return;
        }

        if (isJumping)
        {
            jumpTimer += dt;

            if (jumpHeld && jumpTimer < jumpDuration)
            {
                CurrentVerticalSpeed = jumpPower;
                return;
            }
            else
            {
                isJumping = false;
                isFalling = true;
                fallTimer = 0f;
            }
        }

        if (isFalling)
        {
            fallTimer += dt;

            float t = Mathf.Clamp01(fallTimer / gravityAccelTime);
            float fallSpeed = Mathf.Lerp(0f, maxFallSpeed, t);
            CurrentVerticalSpeed = -fallSpeed;
        }
    }

    public void ForceFall()
    {
        isJumping = false;
        isFalling = true;
        fallTimer = 0f;
    }

    public void StartJump()
    {
        isJumping = true;
        isFalling = false;
        jumpTimer = 0f;
    }
    private void Reset()
    {
        isJumping = false;
        isFalling = false;
        jumpTimer = 0f;
        fallTimer = 0f;
        CurrentVerticalSpeed = 0f;
    }
}

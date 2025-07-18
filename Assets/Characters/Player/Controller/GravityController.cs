using UnityEngine;

[RequireComponent(typeof(PlayerStatus), typeof(Collider2D))]
public class GravityController : MonoBehaviour
{
    private PlayerStatus status;

    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] private float maxFallSpeed = 50f;
    [SerializeField] private float accelTime = 0.25f;
    [SerializeField] private float gravityScale = 20f;

    private float fallTimer = 2.5f;
    public float Gravity { get; private set; }

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    public void Jump()
    {
        if (status.isGrounded)
        {
            Gravity = jumpVelocity;
            fallTimer = 0f;
        }
    }

    public void UpdateGravity(float dt)
    {
        if (status.isGrounded)
        {
            Gravity = 0f;
            fallTimer = 0f;
        }
        else
        {
            fallTimer += dt;
            float t = Mathf.Clamp01(fallTimer / accelTime);
            float currMaxFall = Mathf.Lerp(0f, maxFallSpeed, t);

            Gravity -= gravityScale * dt;
            Gravity = Mathf.Max(Gravity, -currMaxFall);
        }
    }
}


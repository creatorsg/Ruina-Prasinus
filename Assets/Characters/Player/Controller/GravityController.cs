using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerStatus))]
public class GravityController : MonoBehaviour
{
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float maxFallSpeed = -30f;
    [SerializeField] private float jumpVelocity = 15f;

    private Rigidbody2D rb;
    private PlayerStatus status;  
    private float verticalVelocity; 
    public float VerticalVelocity => verticalVelocity;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<PlayerStatus>();
        rb.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        UpdateGravity(Time.fixedDeltaTime);
        ApplyVelocity();
    }

    public void Jump()
    {
        if (status.isGrounded)
        {
            verticalVelocity = jumpVelocity;
        }
    }

    private void UpdateGravity(float deltaTime)
    {
        if (status.isGrounded && verticalVelocity <= 0f)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += gravity * deltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed);
        }
    }

    private void ApplyVelocity()
    {
        Vector2 v = rb.linearVelocity;
        v.y = verticalVelocity;
        rb.linearVelocity = v;
    }
}

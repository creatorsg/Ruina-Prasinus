using UnityEngine;

public class MovePlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private ModelMove model;
    private initialState state;
    private MoveStatus status;

    private Rigidbody2D rb;
    private bool isJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetModel(ModelMove m) => model = m;
    public void SetState(initialState s) => state = s;
    public void SetStatus(MoveStatus s) => status = s;

    public void Move(float moveInput, float dt)
    {
        if (moveInput == 0f)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (status.isSlope && status.isGround && !isJump)
            rb.linearVelocity = Vector2.zero;

        if (moveInput != 0f)
        {
            if (moveInput > 0f)
            {
                Vector2 t = new Vector2(
                    status.perp.x * model.moveSpeed * -moveInput * dt,
                    status.perp.y * model.moveSpeed * -moveInput * dt
                );
                transform.Translate(t, Space.World);
            }
            else if (moveInput < 0f)
            {
                Vector2 t = new Vector2(
                    status.perp.x * model.moveSpeed * moveInput * dt,
                    status.perp.y * model.moveSpeed * moveInput * dt
                );
                transform.Translate(t, Space.World);
            }
            else if (!status.isSlope && status.isGround)
            {
                transform.Translate(
                    Vector2.right * model.moveSpeed * dt * Mathf.Abs(moveInput),
                    Space.World
                );
            }
            else
            {
                transform.Translate(
                    Vector2.right * model.moveSpeed * dt * Mathf.Abs(moveInput),
                    Space.World
                );
            }
        }
    }

    public void Jump()
    {
        if (rb.linearVelocity.y <= 0f)
            isJump = false;

        if (status.isGround && InputManager.GetKeyDown("Jump"))
        {
            isJump = true;
            rb.AddForce(Vector2.up * state.jumpPower, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, model.FacingDirection > 0 ? 180f : 0f, 0f);
    }
}

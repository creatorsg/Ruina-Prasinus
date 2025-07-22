using UnityEngine;

public class newPlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Modeldeldel model;
    private PlayerState state;
    private PlayerStatus2 status;

    private Rigidbody2D rb;
    private bool isJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetModel(Modeldeldel m) => model = m;
    public void SetState(PlayerState s) => state = s;
    public void SetStatus(PlayerStatus2 s) => status = s;

    public void Move(float moveInput)
    {
        // 입력 없으면 X축 위치 고정, 회전만 고정
        if (moveInput == 0f)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 언덕에서 이동 전 잔여 속도 제거 (언덕 위, 땅에 있고, 점프 중이 아닐 때)
        if (status.isSlope && status.isGround && !isJump)
            rb.linearVelocity = Vector2.zero;

        if (moveInput != 0f)
        {
            float dt = Time.deltaTime;
            // 올라갈 때
            if (moveInput > 0f)
            {
                Vector2 t = new Vector2(
                    status.perp.x * model.moveSpeed * -moveInput * dt,
                    status.perp.y * model.moveSpeed * -moveInput * dt
                );
                transform.Translate(t, Space.World);
            }
            // 내려갈 때
            else if (moveInput < 0f)
            {
                Vector2 t = new Vector2(
                    status.perp.x * model.moveSpeed * moveInput * dt,
                    status.perp.y * model.moveSpeed * moveInput * dt
                );
                transform.Translate(t, Space.World);
            }
            // 평지 혹은 공중 이동
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
        if (spriteRenderer != null)
            spriteRenderer.flipX = model.FacingDirection < 0;
    }
}

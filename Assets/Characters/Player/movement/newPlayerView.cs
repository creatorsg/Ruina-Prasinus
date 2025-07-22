using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class newPlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Modeldeldel model;
    private PlayerState state;

    private Rigidbody2D rb;
    private PlayerStatus2 status;

    private bool isJump;
    public void SetModel(Modeldeldel m) => model = m;
    public void SetState(PlayerState s) => state = s;
    public void SetStatus(PlayerStatus2 s) => status = s;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        status = new PlayerStatus2();
    }


    public void Move(float moveinput, float moveSpeed, Vector2 perp)
    {
        if (moveinput == 0)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;


        if (status.isSlope && status.isGround && !isJump)
            rb.linearVelocity = Vector2.zero;
        if (moveinput > 0)
            transform.Translate(new Vector2(perp.x * moveSpeed * -moveinput * Time.deltaTime, perp.y * moveSpeed * moveinput * Time.deltaTime));
        else if (moveinput < 0)
            transform.Translate(new Vector2(perp.x * moveSpeed * -moveinput * Time.deltaTime, perp.y * moveSpeed * moveinput * Time.deltaTime));
        else if (!status.isSlope && status.isGround)
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime * Mathf.Abs(moveinput));
        else
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime * Mathf.Abs(moveinput));
    }

    public void Jump()
    {
        if (rb.linearVelocity.y <= 0)
            isJump = false;

        if (status.isGround)
        {
            if (InputManager.GetKeyDown("Jump"))
            {
                isJump = true;
                rb.AddForce(Vector2.up * state.jumpPower, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = model.FacingDirection < 0;
    }
}
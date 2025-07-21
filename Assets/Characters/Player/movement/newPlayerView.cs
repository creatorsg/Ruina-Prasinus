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
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        status= new PlayerStatus2();
    }


    public void Move(float moveinput, float moveSpeed, Vector2 perp)
    {
        if (moveinput == 0)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;


        if (status.isSlope && status.isGround)
            rb.linearVelocity = Vector2.zero;
        if (moveinput > 0)
            transform.Translate(new Vector2(perp.x * moveSpeed * -moveinput * Time.deltaTime, perp.y * moveSpeed * -moveinput * Time.deltaTime));
        else if (moveinput < 0)
            transform.Translate(new Vector2(perp.x * moveSpeed * -moveinput * Time.deltaTime, perp.y * moveSpeed * -moveinput * Time.deltaTime));
        else
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime * Mathf.Abs(moveinput));
    }

    public void Jump()
    {
         rb.AddForce(Vector2.up * state.jumpPower, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = model.FacingDirection < 0;
    }

    public void SetModel(Modeldeldel m) => model = m;
    public void SetState(PlayerState s) => state = s;
}

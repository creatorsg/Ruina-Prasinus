using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player_View : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateWalk(float speed)
    {
        transform.position += Vector3.right * speed * Time.fixedDeltaTime;
    }

    public void UpdateDash(float speed)
    {
        transform.position += Vector3.right * speed * Time.fixedDeltaTime;
    }

    public void JumpImpulse(float speed)
    {
        if (rb == null) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
    }

    public void ContinueJump(float speed)
    {
        if (rb == null) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, speed);
    }
}

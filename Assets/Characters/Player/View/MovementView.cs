using UnityEngine;

public class MovementView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private MovementModel model;

    public void SetModel(MovementModel m) => model = m;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 v = rb.linearVelocity;

        v.x = model.Velocity.x;

        rb.linearVelocity = v;
        if (spriteRenderer != null)
            spriteRenderer.flipX = model.FacingDirection < 0;
    }
}


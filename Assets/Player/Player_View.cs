using UnityEngine;
using System.Collections;

public class Player_View : MonoBehaviour
{
    public Rigidbody2D rb;
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }
    public void UpdateWalk(float speed, int direction)
    {
        Vector2 vel = rb.linearVelocity;
        vel.x = speed * direction;
        rb.linearVelocity = vel;
        if (speed > 0) transform.localScale = new Vector3(direction, 1, 1);
    }

    public void UpdateDash(float addedSpeed, int direction)
    {
        Vector2 vel = rb.linearVelocity;
        vel.x += addedSpeed * direction;
        rb.linearVelocity = vel;
    }

    public void Jump(float speed)
    {
        Vector2 vel = rb.linearVelocity;
        vel.y = speed;
        rb.linearVelocity = vel;
    }
}

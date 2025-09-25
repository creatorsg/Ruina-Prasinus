using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy2Move : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 1f;
    public float moveTime = 2f;
    public float stopDuration = 0.5f;
    public int direction = 1;

    private Rigidbody2D rb;
    private float moveTimer = 0f;
    private bool isStopped = false;

    [HideInInspector] public bool FirstDetect = false;
    [HideInInspector] public bool StopMove = false;

    private TimerHandler timerHandler = new TimerHandler();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (!FirstDetect)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (StopMove || isStopped)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        moveTimer += Time.fixedDeltaTime;
        if (moveTimer >= moveTime)
        {
            moveTimer = 0f;
            direction *= -1;
            FlipSprite();
            StopForDirectionChange();
        }
    }

    private void Update()
    {
        timerHandler.UpdateTimers(Time.deltaTime);
    }

    public void StopForDirectionChange()
    {
        if (!isStopped)
        {
            isStopped = true;
            timerHandler.AddTimer(stopDuration, () => isStopped = false);
        }
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}

using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerState state;
    public UnityEvent OnHitStarted;
    public UnityEvent OnInvincibleStarted;

    public bool isGrounded {  get; private set; }
    public bool isHit { get; private set; }
    public bool isInvincible { get; private set; }

    private float hitTimer;
    private float invincibleTimer;

    void Update()
    {
        if (isHit)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= state.freezeDuration)
            {
                isHit = false;
                hitTimer = 0f;

                isInvincible = true;
                invincibleTimer = 0f;
                OnInvincibleStarted?.Invoke();
            }
        }
        else if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer >= state.invincibleDuration)
            {
                isInvincible = false;
                invincibleTimer = 0f;
            }
        }
    }

    public void BeginHit()
    {
        if (isHit || isInvincible) return;
        isHit = true;
        hitTimer = 0f;
        OnHitStarted?.Invoke();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
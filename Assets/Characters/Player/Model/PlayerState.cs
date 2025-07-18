using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    public bool IsGrounded { get; private set; }
    public bool IsHited { get; private set; }
    public bool isInvincible { get; private set; }
    public bool isDashing { get; private set; }

    public UnityEvent OnHitStarted;
    public UnityEvent OnInvincibleStarted;

    private float freezeTimer = 0f;
    private float invincibleTimer = 0f;

    private void Update()
    {
        if(IsHited)
        {
            freezeTimer += Time.deltaTime;
            if (freezeTimer >= playerData.freezeDuration)
            {
                IsHited = false;
                freezeTimer = 0f;

                isInvincible = true;
                invincibleTimer = 0f;
            }
            else if (isInvincible)
            {
                invincibleTimer += Time.deltaTime;
                if (invincibleTimer >= playerData.invincibleDuration)
                {
                    isInvincible = false;
                    invincibleTimer = 0f;
                }
            }
        }
    }

    public void BeginHit()
    {
        if (IsHited || isInvincible) return;

        IsHited = true;
        freezeTimer = 0f;
        OnHitStarted?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = false;
    }


}
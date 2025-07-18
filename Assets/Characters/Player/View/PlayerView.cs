using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Transform tf;

    void Awake() => tf = transform;

    public void RenderMovement(Vector2 position, int facing, float speed)
    {
        tf.position = position;
        animator.SetFloat("Speed", speed);
        animator.SetInteger("Facing", facing);
    }

    public void PlayHit() => animator.SetTrigger("Hit");
    public void PlayInvincible() => animator.SetBool("Invincible", true);
    public void StopInvincible() => animator.SetBool("Invincible", false);
}
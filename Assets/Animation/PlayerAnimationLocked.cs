using UnityEngine;

public class PlayerAnimationLock : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void LockAnimation()
    {
        animator.SetBool("isLocked", true);
    }

    public void UnlockAnimation()
    {
        animator.SetBool("isLocked", false);
    }
}
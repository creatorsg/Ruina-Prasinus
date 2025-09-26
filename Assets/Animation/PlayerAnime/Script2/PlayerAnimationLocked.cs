using UnityEngine;

public class AnimationLock : MonoBehaviour
{
    private Animator animator;
    private bool isLocked;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void LockAnimation()
    {
        isLocked = true;
    }

    public void UnlockAnimation()
    {
        isLocked = false;
    }

    public void PlayAnimation(string stateName)
    {
        if (!isLocked)
        {
            animator.Play(stateName);
        }
    }
}

using UnityEngine;

public class PlayerAnimationLock : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 첫 프레임에서 호출
    public void LockAnimation()
    {
        animator.SetBool("isLocked", true);
        Debug.Log("애니메이션 잠금 ON");
    }

    // 마지막 프레임에서 호출
    public void UnlockAnimation()
    {
        animator.SetBool("isLocked", false);
        Debug.Log("애니메이션 잠금 OFF");
    }
}

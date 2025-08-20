using UnityEngine;

public class Enemy2Animation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 이동 상태 설정
    public void SetMoveBool(bool isMove)
    {
        animator.SetBool("isMove", isMove);

    }


    public void SetAttackTrigger()
    {
        
    }

}

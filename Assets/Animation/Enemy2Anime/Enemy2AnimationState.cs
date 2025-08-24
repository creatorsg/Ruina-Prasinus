using UnityEngine;

public class Enemy2AnimationState : MonoBehaviour
{
    private Animator animator;
    private Enemy2Move State;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 이동 상태 설정
    public void SetMoveBool(bool isMove)
    {
        animator.SetBool("isMove", isMove);
    }

    // 공격 트리거 발동
    public void SetChargingStartTrigger()
    {
        animator.SetTrigger("Charging_Start");
    }


    // 차징 시작 여부 설정
    public void SetCharging(bool isCharging)
    {
        animator.SetBool("Charging", isCharging);
    }

    // 발사 상태 설정
    public void SetLaunched(bool isLaunched)
    {
        Debug.Log("SetLaunched: " + isLaunched);
        animator.SetBool("Launched", isLaunched);
    }


}

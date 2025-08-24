using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Enemy2Detect))]
public class Enemy2Animation : MonoBehaviour
{
    private Animator animator;
    private Enemy2Detect detector;

    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        detector = GetComponent<Enemy2Detect>();

        
    }

    

    private void Update()
    {
        // SubState 기준으로 애니메이션 전환
        switch (detector.CurrentSubState)
        {
            case Enemy2Detect.SubState.Chase:
                
                animator.SetBool("Charging", false);
                animator.SetBool("Launched", false);
                break;

            case Enemy2Detect.SubState.Charging:
                if (!animator.GetBool("Charging")) // 처음 진입할 때만
                {
                    animator.SetTrigger("Charging_Start");
                }
                animator.SetBool("Charging", true);
                animator.SetBool("Launched", false);
                break;

            case Enemy2Detect.SubState.Shoot:
                animator.SetBool("Charging", false);
                animator.SetBool("Launched", true);
                break;

            default:
                animator.SetBool("Charging", false);
                animator.SetBool("Launched", false);

                break;
        }

    }
}

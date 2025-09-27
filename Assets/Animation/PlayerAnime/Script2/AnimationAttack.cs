using UnityEngine;

public class AnimationAttack : MonoBehaviour
{
    [SerializeField] private JangpungController jangpungController;
    [SerializeField] private AnimationTotal animationTotal;

    private bool isUp;
    private bool isDown;
    private bool isWalk;

    private bool isGround;

    public int AttackNum { get; private set; } = 0;

    private Animator animator;

    private enum PlayerState { Ground, Air }

    void Awake()
    {
        jangpungController.OnAttack += Attack;
        jangpungController.OnLookUp += HandleLookUp;
        jangpungController.OnLieDown += HandleLieDown;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animationTotal != null)
        {
            isGround = animationTotal.isGround;
            isWalk = animationTotal.isWalk;
        }
    }

    private void Attack()
    {
        // Ground
        if (isGround)
        {
            if (isWalk)
            {
                if (AttackNum == 0)
                {
                    animator.Play("RunningAttack1");
                    AttackNum = 1;
                }
                else
                {
                    animator.Play("RunningAttack2");
                    AttackNum = 0;
                }
            }
            else
            {
                if (AttackNum == 0)
                {
                    animator.Play("GroundAttack1");
                    AttackNum = 1;
                }
                else
                {
                    animator.Play("GroundAttack2");
                    AttackNum = 0;

                }
            }
        }

        else
        {
            // Air
            if (isUp)
            {
                animator.Play("TopAttack");
            }
            else if (isDown)
            {
                animator.Play("UnderAttack");
            }
            else
            {
                animator.Play("JumpAttack");
            }
        }

        

    }

    private void HandleLookUp(bool up)
    {
        isUp = up;
    }

    private void HandleLieDown(bool down)
    {
        isDown = down;
    }

    
}

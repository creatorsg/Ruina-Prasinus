using UnityEngine;
using System.Collections;

public class MovementAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private YDeltaChecker yDeltaChecker;
    [SerializeField] private AnimationTotal animationTotal;
    [SerializeField] private AnimationAttackNotifier attackNotifier;
    [SerializeField] private JangpungController jangpungController;

    private bool isGround;
    private bool wasGround;             // 이전 프레임의 착지 여부 체크
    private string currentState;        // 현재 재생 중인 애니메이션 상태
    private string calculatedState;     // 매 프레임 계산된 상태
    private bool isIntermediatePlaying; // Land, Stop 등 사이 상태 재생중 여부

    private void Awake()
    {
        animator = GetComponent<Animator>();
        yDeltaChecker = GetComponent<YDeltaChecker>();
        animationTotal = GetComponent<AnimationTotal>();

        jangpungController.OnLookUp += LookUp;
        jangpungController.OnLieDown += SitDown;
    }

    private void Update()
    {
        if (animationTotal == null) return;

        // -------------------------
        // 1. 상태 계산 (계속 갱신)
        // -------------------------
        wasGround = isGround;
        isGround = animationTotal.isGround;
        var upDownState = yDeltaChecker.CurrentState;

        // 사이상태 재생중이면 상위 상태 계산 무시
        if (isIntermediatePlaying) return;

        if (isGround)
        {
            // 착지 순간 Land 실행
            if (!wasGround && isGround)
            {
                StartCoroutine(PlayIntermediate("Land", "Idle", 0.3f));
                return;
            }

            if (animationTotal.isWalk)
            {
                if (calculatedState != "Running" && calculatedState != "RunningStart")
                {
                    StartCoroutine(PlayRunningStartOneFrame(0.1f));
                }
            }
            else if (animationTotal.isDash)
            {
                calculatedState = "Dash";
            }
            else
            {
                // 이전에 걷기/대시 상태였다면 Stop 실행
                if (currentState == "Running" || currentState == "RunningStart" || currentState == "Dash")
                {
                    StartCoroutine(PlayIntermediate("Stop", "Idle", 0.2f));
                    return;
                }

                calculatedState = "Idle";
            }

        }
        else
        {
            if (upDownState == YDeltaChecker.UpDownState.Up)
                calculatedState = "Up";
            else if (upDownState == YDeltaChecker.UpDownState.Down)
                calculatedState = "Down";
        }

        // -------------------------
        // 2. 애니메이션 출력
        // -------------------------
        if (attackNotifier != null && attackNotifier.IsAttacking)
        {
            if (currentState != "Attack")
                ChangeAnimation("Attack", force: true);
            return;
        }

        if (!string.IsNullOrEmpty(calculatedState) && currentState != calculatedState)
        {
            ChangeAnimation(calculatedState);
        }
    }

    private IEnumerator PlayRunningStartOneFrame(float duration)
    {
        animator.Play("RunningStart");
        calculatedState = "RunningStart";
        currentState = "RunningStart";

        yield return new WaitForSeconds(duration);

        if (animationTotal.isWalk && isGround)
        {
            animator.Play("Running");
            calculatedState = "Running";
            currentState = "Running";
        }
        else
        {
            // duration 안에 멈춰버리면 Stop 실행
            StartCoroutine(PlayIntermediate("Stop", "Idle", 0.2f));
        }
    }


    /// <summary>
    /// Land, Stop 같은 사이 상태 실행
    /// </summary>
    private Coroutine intermediateCoroutine; // 현재 사이 상태 코루틴 저장

    private IEnumerator PlayIntermediate(string intermediate, string nextState, float duration)
    {
        // Stop은 중복 실행 허용 → 이전 코루틴 강제 중지
        if (intermediate == "Stop" && intermediateCoroutine != null)
        {
            StopCoroutine(intermediateCoroutine);
            isIntermediatePlaying = false;
        }

        // 이미 다른 사이상태 재생중이라면 그냥 무시
        if (isIntermediatePlaying)
            yield break;

        isIntermediatePlaying = true;

        animator.Play(intermediate);
        currentState = intermediate;

        yield return new WaitForSeconds(duration);

        ChangeAnimation(nextState, force: true);

        isIntermediatePlaying = false;
        intermediateCoroutine = null;
    }


    public void OnAttackAnimationEnd()
    {
        if (!string.IsNullOrEmpty(calculatedState))
            ChangeAnimation(calculatedState, force: true);
    }

    private void ChangeAnimation(string newState, bool force = false)
    {
        if (!force && currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }

    private void LookUp(bool up)
    {
        if (!isGround) return;
        if (up)
            animator.Play("LookUp");
    }

    private void SitDown(bool down)
    {
        animator.SetBool("isSitting", down);
    }
}

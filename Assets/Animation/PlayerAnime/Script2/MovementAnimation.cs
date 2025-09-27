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
    private bool wasGround;
    private string currentState;
    private bool isIntermediatePlaying;
    private Coroutine dashCoroutine;

    private string lastCalculatedState;
    private Coroutine intermediateCoroutine;

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

        wasGround = isGround;
        isGround = animationTotal.isGround;

        // Dash, Attack, Intermediate 재생 중이면 Update에서 애니메이션 변경 금지
        if (isIntermediatePlaying || (attackNotifier != null && attackNotifier.IsAttacking) || dashCoroutine != null)
            return;

        // 착지 시 Land 재생
        if (!wasGround && isGround)
        {
            StartCoroutine(PlayIntermediate("Land", "Idle", 0.3f));
            return;
        }

        // Dash 시작
        if (animationTotal.isDash && dashCoroutine == null)
        {
            dashCoroutine = StartCoroutine(PlayDashSequence());
            return;
        }

        // Stop 조건: Running -> 멈춤
        if ((currentState == "Running" || currentState == "RunningStart") && !animationTotal.isWalk)
        {
            StartCoroutine(PlayStopIntermediate(0.2f));
            return;
        }

        // 현재 상태 계산
        string newState = CalculateState();

        // 상태가 바뀌었을 때만 애니메이션 재생
        if (newState != lastCalculatedState)
        {
            ChangeAnimation(newState);
            lastCalculatedState = newState;
        }
    }

    private string CalculateState()
    {
        if (!isGround)
        {
            return yDeltaChecker.CurrentState == YDeltaChecker.UpDownState.Up ? "Up" : "Down";
        }

        if (animationTotal.isWalk)
        {
            if (currentState != "Running" && currentState != "RunningStart")
            {
                StartCoroutine(PlayRunningStartOneFrame(0.1f));
                return "RunningStart";
            }
            return "Running";
        }

        return "Idle";
    }

    private IEnumerator PlayRunningStartOneFrame(float duration)
    {
        animator.Play("RunningStart");
        currentState = "RunningStart";

        yield return new WaitForSeconds(duration);

        if (animationTotal.isWalk && isGround)
        {
            animator.Play("Running");
            currentState = "Running";
        }
        else
        {
            StartCoroutine(PlayStopIntermediate(0.2f));
        }
    }

    private IEnumerator PlayStopIntermediate(float duration)
    {
        if (isIntermediatePlaying) yield break;

        isIntermediatePlaying = true;
        animator.Play("Stop");
        currentState = "Stop";

        float timer = 0f;
        while (timer < duration)
        {
            if (animationTotal.isWalk || animationTotal.isDash || (attackNotifier != null && attackNotifier.IsAttacking))
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        isIntermediatePlaying = false;

        if (!animationTotal.isWalk && !animationTotal.isDash && !(attackNotifier != null && attackNotifier.IsAttacking))
        {
            ChangeAnimation("Idle", force: true);
            lastCalculatedState = "Idle";
        }
    }

    private IEnumerator PlayDashSequence()
    {
        // Intermediate가 재생 중이면 강제로 종료
        if (intermediateCoroutine != null)
        {
            StopCoroutine(intermediateCoroutine);
            intermediateCoroutine = null;
            isIntermediatePlaying = false;
        }

        animator.Play("ToDash");
        currentState = "ToDash";

        yield return null;
        float toDashLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(toDashLength);

        animator.Play("DashRepeat");
        currentState = "DashRepeat";

        while (animationTotal.isDash && isGround)
        {
            yield return null;
        }

        // Dash 종료 후 상태 판단
        if (isGround)
        {
            if (animationTotal.isWalk)
            {
                // 걷기 입력이 있으면 바로 Running으로
                animator.Play("Running", 0, 0f);
                currentState = "Running";
                lastCalculatedState = "Running";
            }
            else
            {
                // 걷기 입력 없으면 Idle로
                StartCoroutine(PlayIntermediate("ToIdle", "Idle", 0.5f));
            }
        }
        else
        {
            var upState = yDeltaChecker.CurrentState == YDeltaChecker.UpDownState.Up ? "Up" : "Down";
            animator.Play(upState, 0, 0f);
            currentState = upState;
            lastCalculatedState = upState;
        }

        dashCoroutine = null;
    }

    private IEnumerator PlayIntermediate(string intermediate, string nextState, float duration)
    {
        if (intermediateCoroutine != null)
        {
            StopCoroutine(intermediateCoroutine);
            isIntermediatePlaying = false;
        }

        if (isIntermediatePlaying) yield break;

        isIntermediatePlaying = true;
        intermediateCoroutine = StartCoroutine(IntermediateRoutine(intermediate, nextState, duration));
    }

    private IEnumerator IntermediateRoutine(string intermediate, string nextState, float duration)
    {
        animator.Play(intermediate);
        currentState = intermediate;

        yield return new WaitForSeconds(duration);

        ChangeAnimation(nextState, force: true);
        lastCalculatedState = nextState;

        isIntermediatePlaying = false;
        intermediateCoroutine = null;
    }

    public void OnAttackAnimationEnd()
    {
        if (animationTotal.isDash)
        {
            if (dashCoroutine == null)
                dashCoroutine = StartCoroutine(PlayDashSequence());
            return;
        }

        string newState = CalculateState();
        ChangeAnimation(newState, force: true);
        lastCalculatedState = newState;
    }

    private void ChangeAnimation(string newState, bool force = false)
    {
        if (!animator.HasState(0, Animator.StringToHash(newState)))
        {
            Debug.LogWarning($"Animator에 상태 없음: {newState}");
            return;
        }

        if (!force && currentState == newState) return;

        animator.Play(newState, 0, 0f);
        currentState = newState;
    }

    private void LookUp(bool up)
    {
        if (!isGround) return;
        if (currentState == "Running" || currentState == "RunningStart" || currentState.StartsWith("Dash")) return;

        if (up)
            animator.Play("LookUp");
    }

    private void SitDown(bool down)
    {
        if (!isGround) return;

        animator.SetBool("isSitting", down);
    }
}

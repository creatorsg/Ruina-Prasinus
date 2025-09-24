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

    private Coroutine dashCoroutine;

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
                if (dashCoroutine == null)
                {
                    dashCoroutine = StartCoroutine(PlayDashSequence());
                }
            }
            else
            {
                if (!isIntermediatePlaying)   // ⭐ Idle 계산 막기
                {
                    if (currentState == "Running" || currentState == "RunningStart")
                    {
                        StartCoroutine(PlayIntermediate("Stop", "Idle", 0.2f));
                        return;
                    }

                    if (currentState.StartsWith("Dash"))
                    {
                        StartCoroutine(PlayIntermediate("ToIdle", "Idle", 0.2f));
                        return;
                    }

                    calculatedState = "Idle";
                }
            }

        }
        else
        {
            if (upDownState == YDeltaChecker.UpDownState.Up)
            {
                // 현재 Down이 아니라면 Up 유지
                if (currentState != "Down")
                    calculatedState = "Up";
            }
            else if (upDownState == YDeltaChecker.UpDownState.Down)
            {
                // 현재가 Up이거나 Down일 때만 Down으로 전환
                if (currentState == "Up" || currentState == "Down")
                    calculatedState = "Down";
            }
        }

        if (attackNotifier != null && attackNotifier.IsAttacking)
        {
            // 현재 공격 중이라면 Animator.Play 호출 금지
            // 대신 논리적 상태만 기록
            currentState = "Attack";

            // Update에서 calculatedState 적용 금지
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

    private IEnumerator PlayDashSequence()
    {
        // 1. ToDash 재생
        animator.Play("ToDash");
        currentState = "ToDash";

        // 한 프레임 대기 후 길이 가져오기
        yield return null;
        float toDashLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(toDashLength);

        // 2. DashRepeat 재생
        animator.Play("DashRepeat");
        currentState = "DashRepeat";

        // DashRepeat 지속 중
        while (animationTotal.isDash && isGround)
        {
            yield return null;
        }

        // 3. Dash 종료 후 상태 판단
        if (isGround)
        {
            if (!animationTotal.isWalk)
            {
                // 걷지 않고 멈춰있다면 ToIdle 재생
                StartCoroutine(PlayIntermediate("ToIdle", "Idle", 0.5f));
                Debug.Log("Dash -> ToIdle" );
            }
            else
            {
                // 걷고 있다면 바로 Running으로
                animator.Play("Running", 0, 0f);
                currentState = "Running";
                calculatedState = "Running";
            }
        }
        else
        {
            // 공중이면 Up/Down 판단
            var upState = yDeltaChecker.CurrentState == YDeltaChecker.UpDownState.Up ? "Up" : "Down";
            animator.Play(upState, 0, 0f);
            currentState = upState;
            calculatedState = upState;
        }

        dashCoroutine = null;
    }




    public void OnAttackAnimationEnd()
    {
        if (isGround)
        {
            if (animationTotal.isWalk)
            {
                // RunningStart → Running 처리
                StartCoroutine(PlayRunningStartOneFrame(0.1f));
            }
            else if (animationTotal.isDash)
            {
                StartCoroutine(PlayDashSequence());
            }
            else
            {
                if (calculatedState != "Running")
                    StartCoroutine(PlayIntermediate("Stop", "Idle", 0.2f));
            }
        }
        else
        {
            calculatedState = yDeltaChecker.CurrentState == YDeltaChecker.UpDownState.Up ? "Up" : "Down";
            ChangeAnimation(calculatedState, force: true);
        }
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

        // Idle 상태에서만 LookUp 실행
        if (currentState != "Idle")
            return;

        if (up)
            animator.Play("LookUp", 0, 0f); // 레이어 명시
    }



    private void SitDown(bool down)
    {   
        if (!isGround) return;

        if (isGround)
            animator.SetBool("isSitting", down);
    }
}

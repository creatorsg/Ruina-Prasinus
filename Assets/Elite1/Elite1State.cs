using UnityEngine;

public class Elite1State : MonoBehaviour
{
    public enum State { Idle, Chase, Attack }

    public State currentState = State.Idle;
    private State previousState;

    private Elite1Detect detector;

    public event System.Action<State> OnStateChanged;

    private void Awake()
    {
        detector = GetComponent<Elite1Detect>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (detector.playerInRange)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                if (!detector.playerInRange)
                    ChangeState(State.Idle);
                else if (detector.playerInRedRange)
                    ChangeState(State.Attack);
                break;

            case State.Attack:
                // Attack은 Action 쪽에서 한 번 실행 후 이전 상태로 복귀시킬 예정
                break;
        }
    }

    public void ChangeState(State newState)
    {
        if (currentState == newState) return;

        previousState = currentState;   
        currentState = newState;

        OnStateChanged?.Invoke(currentState);
    }

    public void ReturnToPreviousState()
    {
        ChangeState(previousState);
    }
}

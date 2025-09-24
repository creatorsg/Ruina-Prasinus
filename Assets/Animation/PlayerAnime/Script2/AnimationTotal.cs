using UnityEngine;

public class AnimationTotal : MonoBehaviour
{
    [SerializeField] private MainPlayer mainPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private MoveStatusHandler moveStatusHandler;
    [SerializeField] private JangpungController jangpungController;

    public bool isGround; // true == 땅에 닿아있음, false == 공중에 떠있음
    public bool isWalk { get; private set; }
    public bool isDash { get; private set; }
    public bool isIdle { get; private set; }

    private MoveBehavior currentState = MoveBehavior.Idle; // 현재 상태 기억

    private void Start()
    {
        if (mainPlayer != null)
        {
            mainPlayer.OnMoveStateChanged += HandleMoveState;
        }

        if (moveStatusHandler != null)
        {
            moveStatusHandler.OnGroundStateChanged += SetGroundBool;
        }
    }

    private void OnDestroy()
    {
        if (mainPlayer != null)
        {
            mainPlayer.OnMoveStateChanged -= HandleMoveState;
        }

        if (moveStatusHandler != null)
        {
            moveStatusHandler.OnGroundStateChanged -= SetGroundBool;
        }
    }

    public void HandleMoveState(MoveBehavior newState)
    {
        // 상태 값 초기화 (Idle 누락 방지)
        isIdle = false;
        isWalk = false;
        isDash = false;

        // 상태 업데이트
        switch (newState)
        {
            case MoveBehavior.Walk:
                isWalk = true;
                break;
            case MoveBehavior.Dash:
                isDash = true;
                break;
            case MoveBehavior.Idle:
                isIdle = true;
                break;
        }

        currentState = newState;
    }

    public void SetGroundBool(bool ground)
    {
        if (isGround != ground)
        {
            isGround = ground;
        }
    }
}

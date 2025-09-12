using UnityEngine;

public class AnimationTotal : MonoBehaviour
{
    [SerializeField] private MainPlayer mainPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private MoveStatusHandler moveStatusHandler;
    [SerializeField] private JangpungController jangpungController;

    public bool isGround; //true == 땅에 닿아있음, false == 공중에 떠있음

    public bool isWalk;
    public bool isDash;
    public bool isIdle;

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
    }

    public void HandleMoveState(MoveBehavior state)
    {
        isIdle = isWalk = isDash = false;

        switch (state)
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
    }

    public void SetGroundBool(bool ground)
    {
        isGround = ground;
    }


}

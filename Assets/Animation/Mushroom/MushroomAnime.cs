using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MushroomAnime : MonoBehaviour
{
    private Animator _animator;

    // 애니메이션 상태 enum
    public enum MushroomState
    {
        Idle,
        Walk,
        Run,
        Warn,
        Charge
    }

    // 현재 상태 변수
    public MushroomState currentState = MushroomState.Idle;

    public MushroomState lastState = MushroomState.Idle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 상태가 바뀌었을 때만 애니메이션 재생
        if (currentState != lastState)
        {
            PlayAnimation(currentState);
            lastState = currentState;
        }
    }

    private void PlayAnimation(MushroomState state)
    {
        switch (state)
        {
            case MushroomState.Idle:
                _animator.Play("idle");
                break;
            case MushroomState.Walk:
                _animator.Play("walk");
                break;
            case MushroomState.Run:
                _animator.Play("run");
                break;
            case MushroomState.Warn:
                _animator.Play("warn");
                break;
            case MushroomState.Charge:
                _animator.Play("charge");
                break;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MushroomAnime : MonoBehaviour
{
    private Animator _animator;
    private PurpleMushroom _mushroom;

    public enum MushroomState
    {
        Idle,
        Walk,
        Run,
        Charge,
        Die
    }

    public MushroomState currentState = MushroomState.Idle;
    private MushroomState lastState = MushroomState.Idle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mushroom = GetComponent<PurpleMushroom>();
    }

    private void Start()
    {
        if (_mushroom != null)
        {
            // PurpleMushroom의 상태 변경 이벤트 구독
            _mushroom.OnStateChanged += OnMushroomStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (_mushroom != null)
        {
            _mushroom.OnStateChanged -= OnMushroomStateChanged;
        }
    }

    private void Update()
    {
        if (currentState != lastState)
        {
            PlayAnimation(currentState);
            lastState = currentState;
        }
    }

    private void PlayAnimation(MushroomState state)
    {
        Debug.Log("Mushroom State: " + state);
        switch (state)
        {
            case MushroomState.Idle:
                _animator.Play("idle");
                break;
            case MushroomState.Walk:
                _animator.Play("walk");
                break;
            case MushroomState.Run:
                _animator.Play("warn");
                break;
            case MushroomState.Charge:
            case MushroomState.Die: // Die 상태도 Charge 애니메이션 재생
                _animator.Play("charge");
                break;
        }
    }



    private void OnMushroomStateChanged(Enemy3Behaviour behaviour)
    {
        switch (behaviour)
        {
            case Enemy3Behaviour.Detect:
                currentState = MushroomState.Idle;
                break;
            case Enemy3Behaviour.Walk:
                currentState = MushroomState.Walk;
                break;
            case Enemy3Behaviour.Rush:
                currentState = MushroomState.Run;
                break;
            case Enemy3Behaviour.Die:
                currentState = MushroomState.Charge; // Die 감지 시 Charge 재생
                break;
            case Enemy3Behaviour.Delete:
                currentState = MushroomState.Idle;
                break;
        }
    }

}

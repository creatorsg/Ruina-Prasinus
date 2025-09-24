using UnityEngine;

public class YDeltaChecker : MonoBehaviour
{
    public enum UpDownState { Up, Down }

    private float previousY;
    private float DeltaY { get; set; }
    public UpDownState CurrentState { get; private set; }

    private bool _isFalling;
    public bool IsFalling => _isFalling;

    private MainPlayer _player; // 플레이어 참조 저장용

    [SerializeField] private float deltaThreshold = 0.01f;
    [SerializeField] private float minUpTime = 0.2f;
    private float upTimer = 0f;

    // ← 여기 Initialize 추가
    public void Initialize(MainPlayer player)
    {
        _player = player;
    }

    void Start()
    {
        previousY = transform.position.y;
    }

    void Update()
    {
        float currentY = transform.position.y;
        DeltaY = currentY - previousY;
        previousY = currentY;

        if (DeltaY > deltaThreshold)
        {
            CurrentState = UpDownState.Up;
            upTimer = minUpTime;
        }
        else if (DeltaY < -deltaThreshold)
        {
            if (upTimer > 0f)
            {
                upTimer -= Time.deltaTime;
                CurrentState = UpDownState.Up;
            }
            else
            {
                CurrentState = UpDownState.Down;
            }
        }
        else
        {
            if (upTimer > 0f)
            {
                upTimer -= Time.deltaTime;
                CurrentState = UpDownState.Up;
            }
            else
            {
                CurrentState = UpDownState.Down;
            }
        }

        _isFalling = CurrentState == UpDownState.Down;
    }
}

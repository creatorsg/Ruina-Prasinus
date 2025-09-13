using UnityEngine;
using UnityEngine.UIElements;

public class MoveHandler : MonoBehaviour
{
    private MainPlayer _player;
    private InputHandler _inputHandler;

    private bool _isWalking, _isDashing;
    private int _moveDirection = 1;
    private RaycastHit _wallHit;
    private float _reaminSpeed;
    public int MoveDirection => _moveDirection;
    public bool IsWalking => _isWalking;
    public bool IsDashing => _isDashing;   
    public float ReaminSpeed => _reaminSpeed;
    public void Initialize(MainPlayer player)
    {
        _player = player;
    }

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, MoveDirection > 0 ? 180f : 0f, 0f);

        if (Mathf.Abs(_inputHandler.MoveInput) > Mathf.Epsilon)
        {
            _player.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            _moveDirection = _inputHandler.MoveInput > 0f ? 1 : -1;
            _isWalking = true;
        }
        else
        {
            _player.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            _isWalking = false;
        }

        if (IsWalking)
        {
            _player.AnimatorManager?.SetMoveBool(true);
        }


        if (!IsWalking)
        {
            _player.AnimatorManager?.SetMoveBool(false);
        }

        if(_player.InputHandler.DashRequested == true)
        {
            _isDashing = true;
        }
        else
        {
            _isDashing = false;
        }

    }

    public void RemainMoveSpeed(float speed)
    {
        _reaminSpeed = speed;
    }

    public Vector2 MovePower(float power)
    {
        Vector2 movePower = new Vector2(_player.MoveStatusHandler.Perp.x * power * -_player.InputHandler.MoveInput * Time.deltaTime,
                                 _player.MoveStatusHandler.Perp.y * power * -_player.InputHandler.MoveInput * Time.deltaTime);

        return movePower;
    }
}

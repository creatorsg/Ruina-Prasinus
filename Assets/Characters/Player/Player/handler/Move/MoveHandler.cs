using UnityEngine;

public class MoveHandler : MonoBehaviour
{
    private MainPlayer _player;
    private InputHandler _inputHandler;

    private bool _isWalking, _isDashing;
    private int _moveDirection = 1;

    public int MoveDirection => _moveDirection;
    public bool IsWalking => _isWalking;
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
            _player.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            _isWalking = false;
        }
    }


}

using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy3MoveHandler : MonoBehaviour
{
    PurpleMushrooms _enemy3;
    private RaycastHit2D _playerHit;
    private GameObject _player;
    private LayerMask _playerMask;
    private float _moveDirection, _detectDistance;

    public float MoveDirection => _moveDirection;
    public void Initialize(PurpleMushrooms enemy3)
    {
        _enemy3 = enemy3;
    }

    private void Awake()
    {
        _playerMask = LayerMask.GetMask("Player");
        _player = GameObject.FindGameObjectWithTag("PlayerBody");
    }

    private void Start()
    {
        _moveDirection = -1f;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, _moveDirection > 0 ? 180f : 0f, 0f);
    }

    public void Detection(float _detectedRange)
    {
        _playerHit = Physics2D.Raycast(transform.position, Vector2.right, 3f, _playerMask);

        if(_playerHit)
        {
            _enemy3.ChangeState(Enemy3Behaviour.Rush);
        }
    }

    public void ChangeDirection()
    {
        _moveDirection = _moveDirection == -1 ? 1 : -1;
    }
}

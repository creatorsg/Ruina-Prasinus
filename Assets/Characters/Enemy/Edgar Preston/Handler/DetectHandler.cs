using System.Runtime.CompilerServices;
using UnityEngine;

public class DetectHandler : MonoBehaviour
{
    private Preston _boss1;
    private GameObject _player;
    private float _direction;
    private LayerMask _groundMask;
    private RaycastHit2D _hit;
    private Vector2 _dir;
    private bool _isGround;

    public Vector2 Dir => _dir;
    public GameObject Player => _player;
    public bool IsGround => _isGround;
    public RaycastHit2D Hit => _hit;

    public LayerMask groundMask => _groundMask;
    public void Initialize(Preston boss1)
    {
        _boss1 = boss1;
    }

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        _dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        CheckGround();
    }


    public void CheckGround()
    {
        _isGround = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1.2f, _groundMask);
    }

    public void DetectPlayer()
    {
        if (_player != null)
        {
            if (_player.transform.position.x < transform.position.x)
            {
                _dir = Vector2.right;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                _dir = Vector2.left;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.2f);
    }
}

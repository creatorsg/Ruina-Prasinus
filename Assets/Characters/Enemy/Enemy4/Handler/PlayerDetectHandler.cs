using UnityEngine;

public class PlayerDetectHandler : MonoBehaviour
{
    private FlowerCannon _enemy4;
    private bool _playerHit;
    private LayerMask _playerMask;
    private Vector2 _dir;

    public bool PlayerHit => _playerHit;

    public void Initialize(FlowerCannon enemy4)
    {
        _enemy4 = enemy4;
    }
    private void Awake()
    {
        _playerMask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        AttackDistance();
    }

    public void AttackDistance()
    {
        _dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        _playerHit = Physics2D.Raycast(transform.position, _dir, 10f, _playerMask);
    }

    private void OnDrawGizmos()
    {
        Vector2 dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, dir * 10f);
    }   
}

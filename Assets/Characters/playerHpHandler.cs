using Player;
using UnityEngine;

public class playerHpHandler : MonoBehaviour
{
    MainPlayer _player;
    PolygonCollider2D _hitbox;
    private bool _isHeating = false, _isInvicible = false;
    private float _hp;

    public PolygonCollider2D Hitbox => _hitbox;
    public void Initialize(MainPlayer player, float hp)
    {
        _player = player;
        _hp = hp;
    }

    public void Awake()
    {
        _hitbox = gameObject.GetComponentInChildren<PolygonCollider2D>();
    }

    public void Update()
    {
        Debug.Log(_hp);
    }

    public void Damaged(float damage)
    {
        _hp -= damage;
    }
}

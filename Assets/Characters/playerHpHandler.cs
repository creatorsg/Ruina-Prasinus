using Player;
using UnityEngine;
using System.Collections;

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

    }

    public void Damaged(float damage)
    {
        if (_isHeating == false && _isInvicible == false)
        {
            _hp -= damage;
            _isHeating = true;
            StartCoroutine(HitRoutine());
        }
    }

    private IEnumerator HitRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        _isHeating = false;

        _isInvicible = true;

        yield return new WaitForSeconds(0.5f);
        _isInvicible = false;
    }
}

using Mono.Cecil;
using UnityEngine;

public class AttackHandler : FindChildObject
{
    private FlowerCannon _enemy4;
    private Transform _bulletSpawner;
    private Bullet _bullet;
    private float _bulletSpeed, _cooltime;
    private bool _isCooltime;

    public bool IsCooltime => _isCooltime;
    public void Initialize(FlowerCannon enemy4, float bulletSpeed)
    {
        _enemy4 = enemy4;
        _bulletSpeed = bulletSpeed;
    }

    private void Awake()
    {
        _bulletSpawner = FindChildWithTag(this.transform, "bulletSpawner");
        _bullet = new Bullet();
        _bullet.projectile = Resources.Load<GameObject>("Circle");
    }

    private void Update()
    {
        if (_isCooltime)
        {
            _cooltime += Time.deltaTime;
            if(_cooltime >= 2.5f)
            {
                _cooltime = 0f;
                _isCooltime = false;
            }
        }
    }
    public void ShootBullet()
    {
        Vector2 dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        GameObject obj = Instantiate(_bullet.projectile, _bulletSpawner.position, Quaternion.identity);
        if (obj.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = dir * _bulletSpeed;
        }
    }

    public void CoolTimer()
    {
        _isCooltime = true;
    }
}

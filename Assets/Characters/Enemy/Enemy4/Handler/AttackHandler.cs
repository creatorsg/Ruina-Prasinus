using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : FindChildObject
{
    private FlowerCannon _enemy4;
    private MonsterSound _monsterSound;
    private Transform _bulletSpawner;
    private GameObject bullet;
    private float _bulletSpeed, _cooltime;
    private bool _isCooltime;
    private EnemyAttackHandler _attackHandler;
    private SpriteRenderer _spriteRenderer;
    private float _hp;
    private GameObject _explosion;
    private float _timer;
    public bool IsCooltime => _isCooltime;
    public void Initialize(FlowerCannon enemy4, float bulletSpeed)
    {
        _enemy4 = enemy4;
        _bulletSpeed = bulletSpeed;
    }

    private void Awake()
    {
        _monsterSound = GetComponent<MonsterSound>();
        _bulletSpawner = FindChildWithTag(this.transform, "bulletSpawner");
        _attackHandler = GetComponent<EnemyAttackHandler>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        bullet = Resources.Load<GameObject>("Bullet2");
        _explosion = Resources.Load<GameObject>("DieEffect");

        _hp = 20f;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            StartCoroutine(Blink());
            _attackHandler.Damaged(_hp, 5f);
            _hp = _hp - 5f;
            if (_hp <= 0f)
            {
                _monsterSound.PlayExplodeSound();
                Explode();
                Destroy(gameObject);
            }
        }
    }
    public void ShootBullet()
    {
        Vector2 dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        GameObject obj = Instantiate(bullet, _bulletSpawner.position, Quaternion.identity);
        if (obj.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = dir * _bulletSpeed;
        }
    }
    public IEnumerator Blink()
    {
        Color originalColor = _spriteRenderer.color;
        _spriteRenderer.color = new Color(3f, 3f, 3f, 1f);
        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = originalColor;
    }

    public void CoolTimer()
    {
        _isCooltime = true;
    }

    public void Explode()
    {
        _spriteRenderer.enabled = false;
        GameObject obj = Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(obj, 0.8f);
    }
}

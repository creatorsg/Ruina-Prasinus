using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1AttackHandler : MonoBehaviour
{
    private playerHpHandler _playerHp;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _attackCollider;
    private Butterflymon _enemy1;
    private PauseManager PauseManager;
    private float _hp, _attackPower;
    private float _heatTimer;
    private bool _isHeating, _attacking;
    private GameObject _explosion;
    public bool Attacking => _attacking;

    public PauseManager pause => PauseManager;

    public void Initialize(Butterflymon enemy1, float Hp, float attackPower)
    {
        _enemy1 = enemy1;
        _attackPower = attackPower;
        _hp = Hp;
    }

    private void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("PauseManager");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        PauseManager = obj.GetComponent<PauseManager>();
        _attackCollider = GetComponent<Collider2D>();
        _explosion = Resources.Load<GameObject>("DieEffect");

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _playerHp = playerObject.GetComponent<playerHpHandler>();
        }
    }

    public void Update()
    {
        if (_isHeating)
        {
            _heatTimer += Time.deltaTime;
            if (_heatTimer >= 0.5f)
            {
                _isHeating = false;
                _heatTimer = 0;
            }
        }

        if(PauseManager.Pause)
        {
            Pause();
            _enemy1.ChangeState(EnemyBehavior.Idle);
        }
    }


    public void Attack()
    {
        List<Collider2D> overlapResults = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter(); 
        _attackCollider.Overlap(filter, overlapResults);

        foreach (var hitCollider in overlapResults)
        {
            if (hitCollider == _playerHp.Hitbox)
            {
                Debug.Log("´êÀ½");
                _playerHp.Damaged(_attackPower);
                _attacking = true;
                break; 
            }
        }
    }

    private void Pause()
    {
        _enemy1.Enemy1MoveHandler.enabled = false;
        _enemy1.Enemy1SpawnHandler.enabled = false;
        _enemy1.Rigidbody2D.linearVelocity = Vector2.zero;
    }
    public void AttackEnd()
    {
        _attacking = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && !_isHeating)
        {
            StartCoroutine(Blink());
            _hp -= 10; 
            _isHeating = true;
            Debug.Log($"Àû HP: {_hp}");

            if (_hp <= 0)
            {
                Explode();
                _enemy1.ChangeState(EnemyBehavior.Die);
            }
        }
    }

    public IEnumerator Blink()
    {
        Color originalColor = _spriteRenderer.color;
        _spriteRenderer.color = new Color(3f, 3f, 3f, 1f);
        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = originalColor;
    }

    public void Explode()
    {
        _spriteRenderer.enabled = false;
        GameObject obj = Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(obj, 0.8f);
    }
}
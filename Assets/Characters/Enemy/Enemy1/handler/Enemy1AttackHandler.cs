using System.Collections.Generic;
using UnityEngine;

public class Enemy1AttackHandler : MonoBehaviour
{
    private playerHpHandler _playerHp;
    private Collider2D _attackCollider;
    private Butterflymon _enemy1;
    private float _hp, _attackPower;

    private float _heatTimer;
    private bool _isHeating;

    public void Initialize(Butterflymon enemy1, float Hp, float attackPower)
    {
        _enemy1 = enemy1;
        _attackPower = attackPower;
        _hp = Hp;
    }

    private void Awake()
    {
        _attackCollider = GetComponent<Collider2D>();

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
                _playerHp.Damaged(_attackPower);
                break; 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && !_isHeating)
        {
            _hp -= 10; 
            _isHeating = true;
            Debug.Log($"Àû HP: {_hp}");

            if (_hp <= 0)
            {
                _enemy1.ChangeState(EnemyBehavior.Die);
            }
        }
    }
}
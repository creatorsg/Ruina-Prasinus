using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour, EnemyCombatInterface
{
    private Collider2D _enemyCollider;
    private SpriteRenderer _spriteRenderer;

    protected playerHpHandler _playerHp;
    private float _enemyHp;

    protected virtual void Awake()
    {
        _enemyCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _playerHp = playerObject.GetComponent<playerHpHandler>();
        }
    }

    public void Attack(float _attackPower)
    {
        List<Collider2D> overlapResults = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        _enemyCollider.Overlap(filter, overlapResults);

        foreach (var hitCollider in overlapResults)
        {
            if (hitCollider == _playerHp.Hitbox)
            {
                Debug.Log("¥Í¿Ω");
                _playerHp.Damaged(_attackPower);
                break;
            }
        }
    }

    public void Damaged(float _enemyHp, float _playerAttack)
    {
        _enemyHp -= _playerAttack;
        Debug.Log(_enemyHp);
    }
}

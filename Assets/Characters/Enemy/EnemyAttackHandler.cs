using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour, EnemyCombatInterface
{
    private PolygonCollider2D _enemyCollider;
    private playerHpHandler _playerHp;
    private float _enemyHp; 
    private bool _heatTerm;

    private void Awake()
    {
        _enemyCollider = GetComponent<PolygonCollider2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _playerHp = playerObject.GetComponent<playerHpHandler>();
        }

        _heatTerm = true;
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
                _playerHp.Damaged(_attackPower,transform);
                break;
            }
        }
    }

    public void Damaged(float _enemyHp,float _playerAttack)
    {
        if (_heatTerm)
        {
            _enemyHp -= _playerAttack;
            _heatTerm = false;
            StartCoroutine(DamageTerm());
            Debug.Log(_enemyHp);
        }
    }

    private IEnumerator DamageTerm()
    {
        yield return new WaitForSeconds(0.5f);
        _heatTerm = true;
    }
}

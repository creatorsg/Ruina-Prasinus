using System.Collections.Generic;
using UnityEngine;

public class attackPlayer : MonoBehaviour
{
    private playerHpHandler _playerHp;
    private Collider2D _attackCollider;
    private float _attackPower = 10f;
    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _playerHp = playerObject.GetComponent<playerHpHandler>();
        _attackCollider = GetComponent<Collider2D>();
    }

    public void Update()
    {
        Attack();
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
                Debug.Log("¥Í¿Ω");
                _playerHp.Damaged(_attackPower);
                break;
            }
        }
    }

}

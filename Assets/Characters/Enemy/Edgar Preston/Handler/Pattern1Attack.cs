using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Pattern1Attack : MonoBehaviour
{
    private Preston _boss1;
    private playerHpHandler _playerHp;
    private Transform _pattern1, _pattern2, _pattern3, _pattern4, _player;
    private Collider2D _attack1, _attack2, _attack3, _attack4;

    public void Initialize(Preston boss1)
    {
        _boss1 = boss1;
    }

    private void Awake()
    {
        _pattern1 = transform.Find("pattern1");
        _pattern2 = transform.Find("pattern2");
        _pattern3 = transform.Find("pattern3");
        _pattern4 = transform.Find("pattern4");
        _player = transform.Find("Player");

        _attack1 = _pattern1.GetComponentInChildren<CapsuleCollider2D>();
        _attack2 = _pattern2.GetComponentInChildren<BoxCollider2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        _playerHp = playerObject.GetComponent<playerHpHandler>();
    }

    public void DashAttack()
    {
        Attack(_attack1, 20);
    }

    public void PoundAttack()
    {
        Attack(_attack2, 20);
    }

    public void CrashAttack()
    {
        Attack(_attack3, 20);
    }

    public void CargingPunch(float index)
    {
        if (index == 1)
            Attack(_attack4, 20);
        else if (index == 2)
            Attack(_attack4, 40);
        else if (index == 3)
            Attack(_attack4, 60);
    }

    public void Attack(Collider2D _attackCollider, float _attackPower)
    {
        if (_playerHp == null)
        {
            Debug.LogError("PlayerHpHandler를 찾을 수 없습니다!");
            return;
        }

        List<Collider2D> overlapResults = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        int hitCount = _attackCollider.Overlap(filter, overlapResults);
        if (hitCount > 0)
        {
            foreach (var hitCollider in overlapResults)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    _playerHp.Damaged(_attackPower);
                    break; 
                }
            }
        }
    }
}
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Pattern1Attack : MonoBehaviour
{
    private Preston _boss1;
    private playerHpHandler _playerHp;
    private Transform _pattern1, _pattern2, _pattern3, _pattern4, _player;
    private BoxCollider2D _attack1, _attack2, _attack3, _attack4;
    private Collider2D _attackCollider;
    private GameObject bossCrush;

    public void Initialize(Preston boss1)
    {
        _boss1 = boss1;
    }

    private void Awake()
    {
        bossCrush = Resources.Load<GameObject>("bossCrush");

        _pattern1 = transform.Find("pattern1");
        _pattern2 = transform.Find("pattern2");
        _pattern3 = transform.Find("pattern3");
        _pattern4 = transform.Find("pattern4");
        _player = transform.Find("Player");

        _attack1 = _pattern1.GetComponentInChildren<BoxCollider2D>();
        _attack2 = _pattern2.GetComponentInChildren<BoxCollider2D>();
        _attack3 = _pattern3.GetComponentInChildren<BoxCollider2D>();
        _attack4 = _pattern4.GetComponentInChildren<BoxCollider2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _playerHp = playerObject.GetComponent<playerHpHandler>();
    }

    public void ActivatePattern(int patternNumber)
    {
        _attack1.enabled = false;
        _attack2.enabled = false;
        _attack3.enabled = false;
        _attack4.enabled = false;

        switch (patternNumber)
        {
            case 1:
                _attack1.enabled = true;
                _attackCollider = _attack1;
                break;
            case 2:
                _attack2.enabled = true;
                _attackCollider = _attack2;
                break;
            case 3:
                _attack3.enabled = true;
                _attackCollider = _attack3;
                break;
            case 4:
                _attack4.enabled = true;
                _attackCollider = _attack4;
                break;
            default:
                Debug.LogWarning("없는 패턴" + patternNumber);
                break;
        }
    }

    public void Attack(float _attackPower)
    {
        List<Collider2D> overlapResults = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        _attackCollider.Overlap(filter, overlapResults);

        foreach (var hitCollider in overlapResults)
        {
            if (hitCollider == _playerHp.Hitbox)
            {
                Debug.Log("닿음");
                _playerHp.Damaged(_attackPower);
                break;
            }
        }
    }

    public int RandomPattern()
    {
        int index = 0;

        if(!_boss1.Boss1HpHandelr.Page2)
        {
            index = Random.Range(0, 3) + 1;
        }
        else
        {
            index = Random.Range(0, 4) + 1;
        }

        return index;
    }

    public void DeactivateAllPatterns()
    {
        _attack1.enabled = false;
        _attack2.enabled = false;
        _attack3.enabled = false;
        _attack4.enabled = false;
    }


    public void ADDSize()
    {
        if (_attack4 != null)
        {
            Vector2 currentSize = _attack4.size;
            _attack4.size = currentSize * 1.2f;

            Debug.Log($"_attack4 size increased: {_attack4.size}");
        }
        else
        {
            Debug.LogError("_attack4가 할당되지 않았습니다!");
        }
    }

    public void CrushAttack()
    {
        GameObject crush = Instantiate(bossCrush, _boss1.transform.position, Quaternion.identity);
        Vector3 scale = crush.transform.localScale;
        scale.x = _boss1.DetectHandler.Dir == Vector2.right ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        crush.transform.localScale = scale;

        Destroy(crush, 0.5f);
    }
}
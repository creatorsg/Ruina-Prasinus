using UnityEngine;
using System.Collections;
public class Enemy2Hp : MonoBehaviour
{
    private MonsterSound _sound;
    private EnemyAttackHandler attackHandler;
    private SpriteRenderer _spriteRenderer;
    private GameObject _explosion;

    private float _hp;
    private void Awake()
    {
        attackHandler = GetComponent<EnemyAttackHandler>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sound = GetComponent<MonsterSound>();
        _explosion = Resources.Load<GameObject>("DieEffect");

        _hp = 20f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            StartCoroutine(Blink());
            _hp -= 10;

            if (_hp <= 0)
            {
                _sound.PlayExplodeSound();
                Explode();
                Destroy(gameObject);
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

        // 크기 변경
        obj.transform.localScale = new Vector2(0.3f, 0.3f); 

        Destroy(obj, 0.6f);
    }
}

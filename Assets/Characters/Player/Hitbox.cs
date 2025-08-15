using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private playerHpHandler _hpHandler;

    void Awake()
    {

        _hpHandler = GetComponentInParent<playerHpHandler>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            if (_hpHandler != null)
            {

                _hpHandler.Damaged(10f);
            }
            else
            {
                Debug.LogError("부모에게서 playerHpHandler를 찾을 수 없습니다!", this.gameObject);
            }
        }
    }
}
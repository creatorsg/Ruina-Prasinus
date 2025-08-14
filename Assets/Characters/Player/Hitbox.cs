using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // 실제 체력 로직을 가진 부모의 핸들러를 저장할 변수
    private playerHpHandler _hpHandler;

    void Awake()
    {
        // 내 부모에게서 playerHpHandler를 찾아와 저장합니다.
        // GetComponentInParent<T>()는 부모 오브젝트들을 거슬러 올라가며 컴포넌트를 찾습니다.
        _hpHandler = GetComponentInParent<playerHpHandler>();
    }

    // 이 스크립트는 자식 콜라이더에 붙어있으므로, 여기서 충돌을 감지합니다.
    void OnTriggerEnter2D(Collider2D other)
    {
        // 적의 공격(총알 등)에 맞았다면,
        if (other.CompareTag("EnemyBullet")) // 태그는 예시입니다. 적의 공격에 맞는 태그로 변경하세요.
        {
            // 실제 데미지 처리는 부모에게 넘깁니다.
            if (_hpHandler != null)
            {
                // 여기서 데미지 양을 정하거나, 공격 콜라이더로부터 받아올 수 있습니다.
                // 예시로 10의 데미지를 넘깁니다.
                _hpHandler.Damaged(10f);
            }
            else
            {
                Debug.LogError("부모에게서 playerHpHandler를 찾을 수 없습니다!", this.gameObject);
            }
        }
    }
}
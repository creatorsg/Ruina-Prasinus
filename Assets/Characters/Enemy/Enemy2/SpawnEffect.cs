using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab; // 재생할 이펙트 프리팹
    private DestroyOnHit target;

    private void Awake()
    {
        // 발사체 자신을 감시 대상으로 설정
        target = GetComponent<DestroyOnHit>();
    }

    private void OnEnable()
    {
        if (target != null)
            target.OnDestroyed += SpawnAtPosition;
    }

    private void OnDisable()
    {
        if (target != null)
            target.OnDestroyed -= SpawnAtPosition;
    }

    private void SpawnAtPosition(GameObject destroyedObject)
    {
        if (effectPrefab != null)
        {
            Debug.Log("Spawned effect at: " + destroyedObject.transform.position);
            GameObject effect = Instantiate(effectPrefab, destroyedObject.transform.position, Quaternion.identity);
            
            Destroy(effect, 0.6f);
        }
    }
    public class ExplodeEffectHandler : MonoBehaviour
    {
        public void ShootAtPlayer()
        {
            Debug.Log("ShootAtPlayer called!");
            // 실제 공격 로직 작성
        }
    }
}

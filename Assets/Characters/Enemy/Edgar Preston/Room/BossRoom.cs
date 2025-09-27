using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public GameObject roomHAVE;

    public Transform player;

    private BoxCollider2D roomTrigger;
    private BoxCollider2D[] roomColliders;
    private GameObject currentBoss;
    private bool _isCleared, _bossSpawned;

    private void Awake()
    {
        roomTrigger = GetComponent<BoxCollider2D>();
        roomColliders = roomHAVE.GetComponents<BoxCollider2D>(); // 멤버 변수에 할당
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isCleared && !_bossSpawned && collision.CompareTag("Player"))
        {
            foreach (var col in roomColliders)
            {
                col.enabled = true;
            }
            BossSpawned();
        }
    }

    public void BossSpawned()
    {
        if (bossPrefab != null)
        {
            currentBoss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            _bossSpawned = true;
        }
    }

    public void BossClear()
    {
        foreach (var col in roomColliders)
        {
            col.enabled = false;
        }
        _isCleared = true;
    }

    public void BossReset()
    {
        foreach (var col in roomColliders)
        {
            col.enabled = false;
        }
    }
}

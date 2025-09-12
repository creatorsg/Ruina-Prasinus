using UnityEngine;

public class Elite1Detect : MonoBehaviour
{
    [Header("감지 범위")]
    public float detectionRange = 10f;       // 파란 Gizmo 범위
    public float redRangeFactor = 1.5f;      // 붉은 Gizmo 범위 = detectionRange / 1.5

    private Transform player;
    public bool playerInRange { get; private set; }
    public bool playerInRedRange { get; private set; }

    private void Awake()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (player == null) { FindPlayer(); return; }

        float dist = Vector2.Distance(transform.position, player.position);
        playerInRange = dist <= detectionRange;
        playerInRedRange = dist <= detectionRange / redRangeFactor;
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange / redRangeFactor);
    }
}

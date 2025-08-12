using UnityEngine;

public class WaterOverlapChecker : MonoBehaviour
{
    [Header("���ο� ����")]
    [SerializeField] private float slowPerStep = 0.025f;
    [SerializeField] private float minSubmergePercent = 20f;
    [SerializeField] private float stepPercent = 10f;

    [Header("�߷� ����")]
    [SerializeField] private float maxGravityReduction = 0.5f;

    private Collider2D playerCollider;
    private Rigidbody2D rb;

    private float originalGravityScale;
    private float currentSubmergePercent;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        if (playerCollider == null)
            Debug.LogError("�÷��̾ Collider2D�� �����ϴ�!");

        if (rb != null)
            originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (playerCollider == null || rb == null) return;

        GameObject[] waters = GameObject.FindGameObjectsWithTag("waterPrefep");

        float maxSubmergePercent = 0f;

        foreach (GameObject water in waters)
        {
            Collider2D waterCollider = water.GetComponent<Collider2D>();
            if (waterCollider == null) continue;

            var result = Physics2D.Distance(playerCollider, waterCollider);

            if (result.isOverlapped)
            {
                float penetrationDepth = -result.distance;
                float playerHeight = GetColliderHeight(playerCollider);
                float percentSubmerged = Mathf.Clamp01(penetrationDepth / playerHeight) * 100f;

                maxSubmergePercent = Mathf.Max(maxSubmergePercent, percentSubmerged);
            }
        }

        currentSubmergePercent = maxSubmergePercent;
    }

    void LateUpdate()
    {
        if (rb == null) return;

        if (currentSubmergePercent < minSubmergePercent)
        {
            rb.gravityScale = originalGravityScale;
            return;
        }

        float gravitySlowRatio = 1f - Mathf.Clamp01((currentSubmergePercent - minSubmergePercent) / 80f);
        rb.gravityScale = originalGravityScale * Mathf.Lerp(1f, 1f - maxGravityReduction, 1f - gravitySlowRatio);

        int slowStepCount = Mathf.FloorToInt((currentSubmergePercent - minSubmergePercent) / stepPercent);
        float slowRatio = 1f - (slowStepCount * slowPerStep);

        rb.linearVelocity *= slowRatio;
    }

    float GetColliderHeight(Collider2D col)
    {
        if (col is BoxCollider2D box)
            return box.size.y * Mathf.Abs(col.transform.lossyScale.y);
        else if (col is CircleCollider2D circle)
            return circle.radius * 2f * Mathf.Abs(col.transform.lossyScale.y);
        else if (col is CapsuleCollider2D capsule)
            return capsule.size.y * Mathf.Abs(col.transform.lossyScale.y);

        return 1f;
    }
}

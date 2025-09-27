using UnityEngine;

public class JangpungDestroy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float autoDestroyTime = 0.25f;
    [SerializeField] private string energyBlastEndClipName = "energyBlastEnd";

    private bool hasHit = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // autoDestroyTime�� ������ energyBlastEnd ���
        Invoke(nameof(AutoDestroy), autoDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        if (collision.CompareTag("Ground") || collision.CompareTag("Enemy"))
        {
            hasHit = true;
            LockPositionAndPlayAnimation();
        }
    }

    private void AutoDestroy()
    {
        if (hasHit) return;

        hasHit = true;
        LockPositionAndPlayAnimation();
    }

    private void LockPositionAndPlayAnimation()
    {
        // ��ġ ����
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; // ���� ���� ����
        }

        // �ʿ� �� Transform �̵��� ���� �� ����
        // transform.position = transform.position;

        // �ִϸ��̼� ���
        _animator.Play(energyBlastEndClipName, -1, 0f);
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ��
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;

public class MoveCheack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    private bool isMoving;

    [SerializeField] private float velocityThreshold = 0.05f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Rigidbody2D�� �ӵ��� ���� �� �̻��� ���� ���������� ����
        bool shouldMove = Mathf.Abs(rb.linearVelocity.x) > velocityThreshold;

        if (shouldMove != isMoving)
        {
            isMoving = shouldMove;
            animator.SetBool("isMove", isMoving);
        }
    }
}

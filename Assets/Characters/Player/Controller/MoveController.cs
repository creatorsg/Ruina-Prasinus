using player2;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour
{
    [SerializeField] private InputComponent input;
    [SerializeField] private PlayerStatus status;
    [SerializeField] private PlayerState data;

    public float HorizontalVelocity { get; private set; }

    private MovementModel moveModel = new MovementModel();
    private Rigidbody2D rb;
    private float moveInput = 0f;
    private bool dashReq = false;
    private float originalGravity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;

        input.OnMove += dir => moveInput = dir;
        input.OnDash += () => dashReq = true;
    }

    void Update()
    {
        // �Է¸� Update���� �ް�
        moveModel.Update(moveInput, dashReq, data, Time.fixedDeltaTime);
        dashReq = false;
    }

    void FixedUpdate()
    {
        // 1) ���� ����: ���鿡 �پ� �ְ�, �Է��� 0�� ��
        if (status.isGrounded && Mathf.Approximately(moveInput, 0f))
        {
            // �߷� ����(+���� �غ�) & �ӵ� 0
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 2) �ٽ� ���� �߷����� ����
        if (rb.gravityScale == 0f)
            rb.gravityScale = originalGravity;

        // 3) �Է� ���� ���� X �ӵ� ���
        float vx = (moveModel.CurrentSpeed + moveModel.CurrentDashSpeed)
                   * moveModel.FacingDirection;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);

        // (Optional) ���鿡�� �������� �� Y �ӵ��� 0���� ��� ������
        // if(status.isGrounded) rb.velocity = new Vector2(rb.velocity.x, 0f);
    }
}

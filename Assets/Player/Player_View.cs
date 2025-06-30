// PlayerView.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player_View : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private Player_Model model;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateWalk(float speed)
    {
        transform.position += Vector3.right * speed * Time.fixedDeltaTime;
    }

    public void UpdateDash(float speed)
    {
        transform.position += Vector3.right * speed * Time.fixedDeltaTime;
    }

    public void JumpImpulse(float speed)
    {
        // 기존 속도를 깔끔히 초기화하고
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        // 한 번만 임펄스
        rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
    }

    public void ContinueJump(float speed)
    {
        // 등속 상승: 매 프레임마다 y축 속도 고정
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, speed);
    }
}

using UnityEngine;

public class MoveCheack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float lastX;

    void Start()
    {
        // 시작 시 현재 위치 저장
        lastX = transform.position.x;

        // 인스펙터에서 Animator 연결 안 했으면 자동으로 찾음
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        float currentX = transform.position.x;

        // x 좌표가 달라졌는지 확인
        if (Mathf.Abs(currentX - lastX) > 0.001f)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }

        // 이번 프레임의 위치를 저장
        lastX = currentX;
    }
}

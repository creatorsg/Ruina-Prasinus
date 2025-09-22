using UnityEngine;

public class Pin : MonoBehaviour
{
    [Range(0, 250)]
    [SerializeField] private float pinGacDo = 0f;

    private float previousGacDo = 0f;
    private float initialZRotation; // 초기 Z축 회전 저장

    private void Awake()
    {
        // 오브젝트가 시작할 때 현재 Z축 회전을 기준점으로 저장
        initialZRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        if (pinGacDo != previousGacDo)
        {
            // 초기 각도를 기준으로 -방향 회전
            transform.rotation = Quaternion.Euler(0f, 0f, initialZRotation + pinGacDo);

            previousGacDo = pinGacDo;
        }
    }
}

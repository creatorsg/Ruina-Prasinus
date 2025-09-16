using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Range(0f, 100f)]
    [SerializeField] private float health = 100f;

    public RectTransform healthIndicator;  // 체력바 (UI Image)
    public RectTransform followerImage;    // 바 끝을 따라 움직일 이미지

    private float originalWidth;
    private Vector3 originalFollowerPos;

    private void Awake()
    {
        if (healthIndicator != null)
        {
            originalWidth = healthIndicator.sizeDelta.x;
        }

        if (followerImage != null)
        {
            originalFollowerPos = followerImage.anchoredPosition;
        }
    }

    private void Update()
    {
        if (healthIndicator != null)
        {
            // 체력에 따라 width 조절
            float newWidth = originalWidth * (health / 100f);
            healthIndicator.sizeDelta = new Vector2(newWidth, healthIndicator.sizeDelta.y);

            // followerImage 이동 (pivot이 왼쪽 끝 기준)
            if (followerImage != null)
            {
                // 바 왼쪽 끝 기준 pivot = (0,0.5)일 경우
                followerImage.anchoredPosition = originalFollowerPos + new Vector3(newWidth, 0, 0) + new Vector3(-280, 0, 0);
            }
        }
    }
}

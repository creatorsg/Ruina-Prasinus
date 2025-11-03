using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public RectTransform healthIndicator;
    public RectTransform followerImage;

    private float originalWidth;
    private Vector3 originalFollowerPos;
    private float currentHealth = 100f;

    private void Awake()
    {
        if (healthIndicator != null)
            originalWidth = healthIndicator.sizeDelta.x;
        if (followerImage != null)
            originalFollowerPos = followerImage.anchoredPosition;
    }

    public void Bind(playerHpHandler hpHandler)
    {
        hpHandler.OnHpChanged += UpdateHealth;
    }

    private void UpdateHealth(float newHp)
    {
        currentHealth = newHp;
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (healthIndicator != null)
        {
            float newWidth = originalWidth * (currentHealth / 100f);
            healthIndicator.sizeDelta = new Vector2(newWidth, healthIndicator.sizeDelta.y);

            if (followerImage != null)
                followerImage.anchoredPosition = originalFollowerPos + new Vector3(newWidth, 0, 0) + new Vector3(-280, 0, 0);
        }
    }
}

using UnityEngine;

public class Following_Player : MonoBehaviour
{
    public Transform player;
    public BoxCollider2D cameraBounds;
    public GameObject boundParent;


    private float halfHeight;
    private float halfWidth;
    private Camera cam;

    private bool isTransitioning = false;
    private Vector3 targetPosition;

    void Start()
    {
        cam = Camera.main;
        UpdateCameraHalfSize();
    }

    void LateUpdate()
    {
        if (player == null || cameraBounds == null) return;

        UpdateCameraHalfSize();

        if (!isTransitioning)
        {
            // 일반 이동
            Bounds b = cameraBounds.bounds;
            float minX = b.min.x + halfWidth, maxX = b.max.x - halfWidth;
            float minY = b.min.y + halfHeight, maxY = b.max.y - halfHeight;

            float cx = Mathf.Clamp(player.position.x, minX, maxX);
            float cy = Mathf.Clamp(player.position.y + 1f, minY, maxY);

            transform.position = new Vector3(cx, cy, -10f);
        }
        else
        {
            // x축만 카메라 이동, Time.timeScale 영향 없이
            float newX = Mathf.Lerp(transform.position.x, targetPosition.x, Time.unscaledDeltaTime * 2f);
            float newY = Mathf.Lerp(transform.position.y, targetPosition.y, Time.unscaledDeltaTime * 2f);
            transform.position = new Vector3(newX, newY, -10f);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isTransitioning = false;
                Time.timeScale = 1f; // 카메라 이동 끝나면 슬로우 모션 해제
            }
        }
    }

    // 방 이동 시작 시 호출
    public void TransitionToNewRoom(BoxCollider2D newBounds)
    {
        cameraBounds = newBounds;

        Bounds b = cameraBounds.bounds;
        float minX = b.min.x + halfWidth, maxX = b.max.x - halfWidth;
        float minY = b.min.y + halfHeight, maxY = b.max.y - halfHeight;

        float cx = Mathf.Clamp(player.position.x, minX, maxX);
        float cy = Mathf.Clamp(player.position.y + 1f, minY, maxY);

        targetPosition = new Vector3(cx, cy, -10f);
        isTransitioning = true;

        Time.timeScale = 0f; // 플레이어 느리게 시작
    }

    void UpdateCameraHalfSize()
    {
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }
}

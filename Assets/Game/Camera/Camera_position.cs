using UnityEngine;

public class Following_Player : MonoBehaviour
{
    public Transform player;
    public BoxCollider2D cameraBounds;
    public GameObject boundParent;

    private float halfWidth, halfHeight, moveSpeed = 18f;
    private Camera cam;

    private bool isTransitioning = false;
    private Vector3 targetPosition;

    private void Awake()
    {
        cam = Camera.main;
        UpdateCameraHalfSize();

        if (player != null && cameraBounds != null)
        {
  
            Bounds b = cameraBounds.bounds;
            float minX = b.min.x + halfWidth, maxX = b.max.x - halfWidth;
            float minY = b.min.y + halfHeight, maxY = b.max.y - halfHeight;

            float cx = Mathf.Clamp(player.position.x, minX, maxX);
            float cy = Mathf.Clamp(player.position.y, minY, maxY);

            transform.position = new Vector3(cx, cy, -10f);
        }
    }

    void LateUpdate()
    {
        if (isTransitioning)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.unscaledDeltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isTransitioning = false;
                Time.timeScale = 1f;
            }
        }
        else
        {
            MoveCameraToPlayerInstant();
        }
    }

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

        Time.timeScale = 0f;
    }

    private void MoveCameraToPlayerInstant()
    {
        if (player == null || cameraBounds == null) return;

        Bounds b = cameraBounds.bounds;
        float minX = b.min.x + halfWidth, maxX = b.max.x - halfWidth;
        float minY = b.min.y + halfHeight, maxY = b.max.y - halfHeight;

        float cx = Mathf.Clamp(player.position.x, minX, maxX);
        float cy = Mathf.Clamp(player.position.y + 1f, minY, maxY);

        transform.position = new Vector3(cx, cy, -10f);
    }

    void UpdateCameraHalfSize()
    {
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }
}

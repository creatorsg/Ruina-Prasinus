using UnityEngine;

public class Following_Player : MonoBehaviour
{
    public GameObject boundParent;
    public Transform player;
    public BoxCollider2D cameraBounds;

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
        Transform parentTransform = cameraBounds.transform.parent;

        if (player == null || cameraBounds == null) return;
        UpdateCameraHalfSize();

        if (!isTransitioning)
        {
            Bounds b = cameraBounds.bounds;
            float minX = b.min.x + halfWidth, maxX = b.max.x - halfWidth;
            float minY = b.min.y + halfHeight, maxY = b.max.y - halfHeight;

            float cx = Mathf.Clamp(player.position.x, minX, maxX);
            float cy = Mathf.Clamp(player.position.y + 1f, minY, maxY);

            transform.position = new Vector3(cx, cy, -10f);
        }
        else
        {
            // 전환 중일 때는 targetPosition으로만 보간
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
                isTransitioning = false;
        }

        boundParent = parentTransform != null
              ? parentTransform.gameObject
              : null;
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
    }

    void UpdateCameraHalfSize()
    {
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }
}

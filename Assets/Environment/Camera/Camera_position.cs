using UnityEngine;

public class Following_Player : MonoBehaviour
{
    public Transform player;
    public BoxCollider2D cameraBounds;

    private float halfHeight;
    private float halfWidth;
    private Camera cam;

    public GameObject boundParent;

    void Start()
    {
        cam = Camera.main;
        UpdateCameraHalfSize();
    }

    private void FixedUpdate()
    {
        if (cameraBounds != null)
        {
            Transform parentTransform = cameraBounds.transform.parent;

            boundParent = parentTransform != null
                          ? parentTransform.gameObject
                          : null;
        }
    }

    void LateUpdate()
    {
        if (player == null || cameraBounds == null) return;
        UpdateCameraHalfSize();

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

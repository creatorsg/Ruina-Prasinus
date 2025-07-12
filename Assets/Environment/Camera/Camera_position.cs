using UnityEngine;

public class Following_Player : MonoBehaviour
{
    public Transform player;
    public BoxCollider2D cameraBounds; 

    private float halfHeight;
    private float halfWidth;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        UpdateCameraHalfSize();
    }

    void LateUpdate()
    {
        if (player == null || cameraBounds == null) return;

        UpdateCameraHalfSize();

        Bounds bounds = cameraBounds.bounds;

        float minX = bounds.min.x + halfWidth;
        float maxX = bounds.max.x - halfWidth;
        float minY = bounds.min.y + halfHeight;
        float maxY = bounds.max.y - halfHeight;

        float clampedX = Mathf.Clamp(player.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(player.position.y + 1f, minY, maxY); 

        transform.position = new Vector3(clampedX, clampedY, -10f);
    }

    void UpdateCameraHalfSize()
    {
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }
}


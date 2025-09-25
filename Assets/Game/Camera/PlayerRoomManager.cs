using UnityEngine;

public class PlayerRoomDetector : MonoBehaviour
{
    private Following_Player cameraFollow;
    private Collider2D playerCollider;
    private bool hasTriggered = false;

    void Awake()
    {
        cameraFollow = Camera.main.GetComponent<Following_Player>();
        if (cameraFollow.player != null)
        {
            playerCollider = cameraFollow.player.GetComponent<Collider2D>();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("RoomBound")) return;
        if (playerCollider == null) return;

        Collider2D roomBounds = other.GetComponent<Collider2D>();
        Bounds rb = roomBounds.bounds;

 
        float centerX = rb.center.x;
        float centerY = rb.center.y;

        Vector3 playerPos = cameraFollow.player.position;


        if (rb.Contains(playerCollider.bounds.min) &&
            rb.Contains(playerCollider.bounds.max) &&
            Mathf.Abs(playerPos.x - centerX) < rb.extents.x * 0.95f &&
            Mathf.Abs(playerPos.y - centerY) < rb.extents.y * 0.95f)
        {
            cameraFollow.TransitionToNewRoom(roomBounds);
            hasTriggered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("RoomBound"))
        {
            hasTriggered = false;
        }
    }
}
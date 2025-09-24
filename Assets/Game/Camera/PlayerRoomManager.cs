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

        BoxCollider2D roomBounds = other.GetComponent<BoxCollider2D>();

        if (roomBounds.bounds.Contains(playerCollider.bounds.min) &&
            roomBounds.bounds.Contains(playerCollider.bounds.max))
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

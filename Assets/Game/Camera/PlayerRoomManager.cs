using UnityEngine;

public class PlayerRoomDetector : MonoBehaviour
{
    private Following_Player cameraFollow;

    void Awake()
    {
        cameraFollow = Camera.main.GetComponent<Following_Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RoomBound"))
        {
            cameraFollow.cameraBounds = other.GetComponent<BoxCollider2D>();
        }
    }
}

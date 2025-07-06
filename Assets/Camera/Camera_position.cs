using UnityEngine;

namespace Camera
{
    public class Following_Player : MonoBehaviour
    {
        public Transform player;
        void Start()
        {

        }
        void Update()
        {
            transform.position = player.transform.position + new Vector3(0, 1, -10);
        }
    }
}
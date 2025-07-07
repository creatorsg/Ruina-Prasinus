using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Player_Event : MonoBehaviour
    {
        private Player_Model model = new Player_Model();

        private float Hitcount = 0f;
        void Start()
        {
            GameObject player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            if (Hitcount > 0f)
            {
                Hitcount -= 1f;
            }
        }
        void HandleHit()
        {
            var m = model;

            if (Hitcount > 0f)
            {
                {

                }
            }

        }
    }
}

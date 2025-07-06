using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Player_Event : MonoBehaviour
    {
        private Player_Model model = new Player_Model();

        void Start()
        {
            GameObject player = GameObject.Find("Player");
            Color color = player.GetComponent<Image>().color;
            color.a = 0.5f;
            player.GetComponent<Image>().color = color;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {

        }
        void HandleHit()
        {
            var m = model;

            if (m.isHit)
            {
                Color color = GetComponent<Image>().color;


            }
        }

    }
}

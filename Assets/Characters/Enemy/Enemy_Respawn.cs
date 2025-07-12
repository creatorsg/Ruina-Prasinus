using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy_Respawn : MonoBehaviour
{
    public Tilemap room;
    public bool isDestroyed = false;
    public Vector3 RespawnPoint;
    void Start()
    {
        room = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
        {
            return;
        }
        if (!isDestroyed)
        {

        }
    }
}

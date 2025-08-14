using UnityEngine;

public class Enemy1SpawnHandler : MonoBehaviour
{
    private Butterflymon _enemy1;

    [HideInInspector] public RoomEnemyRespawner roomRespawner;
    public void Initialize(Butterflymon enemy1)
    {
        _enemy1 = enemy1;
    }


    public void Spawn()
    {

    }
    public void CheckDestroyed()
    {
        if (roomRespawner != null)
            roomRespawner.MarkDestroyed(1);

        Destroy(gameObject);
    }
}

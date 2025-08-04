using UnityEngine;
using static RoomEnemyRespawner;

public class EnemyDestroy 
{
    public void EnemyDestory(SpawnInfo spawninfo)
    {
        GameObject.Destroy(spawninfo.enemyPrefab);
    }
}

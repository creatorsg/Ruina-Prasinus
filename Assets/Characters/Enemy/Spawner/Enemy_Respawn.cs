using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyRespawner : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private List<SpawnInfo> spawnInfos;

    private readonly List<GameObject> currentEnemies = new List<GameObject>();

    private Following_Player playerCamera;

    void Awake()
    {
        playerCamera = UnityEngine.Object.FindFirstObjectByType<Following_Player>();

        for (int i = 0; i < spawnInfos.Count; i++)
            currentEnemies.Add(null);
    }

    void Update()
    {
        bool inThisRoom = playerCamera.boundParent == room;

        if (inThisRoom)
        {
            for (int i = 0; i < spawnInfos.Count; i++)
            {
                var info = spawnInfos[i];
                bool hasInstance = currentEnemies[i] != null;
                if (!hasInstance && !info.isDestroyed)
                {
                    var e = Instantiate(info.enemyPrefab, info.spawnPosition, Quaternion.identity);

                    var ed = e.GetComponent<Enemy_Destroy>();
                    if (ed != null)
                    {
                        ed.destroyCheck = i;
                        ed.roomRespawner = this;
                    }

                    currentEnemies[i] = e;
                }
            }
        }
        else
        {
            for (int i = 0; i < currentEnemies.Count; i++)
            {
                if (currentEnemies[i] != null)
                    Destroy(currentEnemies[i]);
                currentEnemies[i] = null;
            }
        }
    }

    public void MarkDestroyed(int index)
    {
        if (index >= 0 && index < spawnInfos.Count)
            spawnInfos[index].isDestroyed = true;
    }
}

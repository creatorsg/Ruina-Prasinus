using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyRespawner : MonoBehaviour
{
    [Header("— 방 구분용 오브젝트")]
    [SerializeField] private GameObject room;

    [Header("— 스폰 정보 목록 (한 칸에 3개 항목 노출)")]
    [SerializeField] private List<SpawnInfo> spawnInfos;

    // 런타임에 생성된 몬스터들
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
                        ed.spawnIndex = i;
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

    [Serializable]
    private class SpawnInfo
    {
        public GameObject enemyPrefab;
        public Vector3 spawnPosition;
        public bool isDestroyed = false;
    }
}

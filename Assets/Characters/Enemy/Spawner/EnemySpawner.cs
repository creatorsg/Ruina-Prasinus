using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private List<SpawnInfo> spawnInfos;
    [SerializeField] private Collider2D monsterSpawnCollider;
    [SerializeField] private GameObject mosterRoom;

    private Following_Player playerCamera;

    private void Update()
    {
        bool InROOM = playerCamera.boundParent == room;
    }

    private void SpawnMonster()
    {
       
    }
}

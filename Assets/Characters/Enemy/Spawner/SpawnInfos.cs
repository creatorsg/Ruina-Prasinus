using System;
using UnityEngine;

[Serializable]
public class SpawnInfos
{
    public GameObject enemyPrefab;
    public Vector3 spawnPosition;
    public bool isDestroyed = false;
}

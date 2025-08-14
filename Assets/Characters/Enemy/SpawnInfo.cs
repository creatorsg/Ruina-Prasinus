using UnityEngine;

[System.Serializable]
public class SpawnInfo
{
    public GameObject enemyPrefab;
    public Vector3 spawnPosition;
    
    [HideInInspector] public bool isDestroyed;
}
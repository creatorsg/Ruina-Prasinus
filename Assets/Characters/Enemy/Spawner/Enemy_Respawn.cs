using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyRespawner : MonoBehaviour
{
    [SerializeField] private GameObject _room;
    [SerializeField] private Collider2D monsterSpawnCollider;

    [SerializeField] private List<SpawnInfo> _spawnInfos;

    private readonly List<GameObject> _currentEnemies = new List<GameObject>();

    private Following_Player _playerCamera;
    private bool isPlayerInSpawnArea = false;
    private void Awake()
    {
        _playerCamera = UnityEngine.Object.FindFirstObjectByType<Following_Player>();

        for (int i = 0; i < _spawnInfos.Count; i++)
            _currentEnemies.Add(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInSpawnArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInSpawnArea = false;
        }
    }

    private void Update()
    {
        bool inThisRoom = _playerCamera.boundParent == _room;

        if (inThisRoom && isPlayerInSpawnArea)
        {
            for (int i = 0; i < _spawnInfos.Count; i++)
            {
                var info = _spawnInfos[i];
                bool hasInstance = _currentEnemies[i] != null;
                if (!hasInstance && !info.isDestroyed)
                {
                    var e = Instantiate(info.enemyPrefab, info.spawnPosition, Quaternion.identity);

                    var ed = e.GetComponent<Enemy_Destroy>();
                    if (ed != null)
                    {
                        ed.destroyCheck = i;
                        ed.roomRespawner = this;
                    }
                    _currentEnemies[i] = e;
                }
            }
        }

        if (!inThisRoom)
        {
            for (int i = 0; i < _currentEnemies.Count; i++)
            {
                if (_currentEnemies[i] != null)
                {
                    Destroy(_currentEnemies[i]);
                    _currentEnemies[i] = null; 
                }
            }
        }
    }

    public void MarkDestroyed(int index)
    {
        if (index >= 0 && index < _spawnInfos.Count)
            _spawnInfos[index].isDestroyed = true;
    }
}

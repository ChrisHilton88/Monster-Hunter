using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager> 
{
    private int _waveCount;

    Coroutine _enemySpawnRoutine;                               // Stores a coroutine so only 1 can play.
    WaitForSeconds _spawnWaitTime = new WaitForSeconds(2);      // Caching the wait time between enemies spawning.

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _endPoint;   

    [SerializeField] private List<SOEnemyWaves> _waveList;      // Contains a list of all the SO wave assets.

    #region
    public Transform EndPoint { get { return _endPoint; } }
    public int WaveCount { get { return _waveCount; } private set { _waveCount = value; } }                  // Counter to control the waves.
    #endregion


    void Start()
    {
        WaveCount = 1;
        if (_enemySpawnRoutine == null)
            _enemySpawnRoutine = StartCoroutine(SpawningEnemiesRoutine());
    }

    // Spawning Enemies Routine
    IEnumerator SpawningEnemiesRoutine()
    {
        Debug.Log("Wave Count: " + WaveCount);

        while(WaveCount < _waveList.Count)
        {
            for (int i = 0; i < _waveList[i].enemyList.Count; i++)              // pass in an enemy count number to determine length of routine 
            {
                EnemyObjectPool.Instance.RequestEnemy();          // Keep requesting a new enemy every iteration.
                yield return _spawnWaitTime;                        // Wait 2 seconds
            }

            WaveCount++;
            Debug.Log("Wave Count: " + WaveCount);
        }

        _enemySpawnRoutine = null;                              // When loop is finished, set the coroutine variable to null so we can run it again.
    }
}
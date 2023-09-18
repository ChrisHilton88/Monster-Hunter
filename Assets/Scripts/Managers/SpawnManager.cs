using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoSingleton<SpawnManager> 
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _endPoint;   

    private Quaternion _spawnRotation = Quaternion.identity;        // Make sure spawn rotation is always facing the door

    WaitForSeconds _spawnWaitTime = new WaitForSeconds(2);      // Caching the wait time between enemies spawning.
    Coroutine _enemySpawnRoutine;                               // Stores a coroutine so only 1 can play.
    [SerializeField] private List<SOEnemyWaves> _waveList;      // Contains a list of all the SO wave assets.


    public Transform EndPoint { get { return _endPoint; } } 
    public int WaveCount { get; private set; }                  // Counter to control the waves.


    void Start()
    {
        WaveCount = 1;

        // When ready, change this so it starts after the first cutscene has finished.
        //if (_enemySpawnRoutine == null)
        //    _enemySpawnRoutine = StartCoroutine(SpawningEnemiesRoutine());
    }

    // Spawning Enemies Routine
    IEnumerator SpawningEnemiesRoutine()
    {
        Debug.Log("Wave Count: " + WaveCount);

        while(WaveCount < _waveList.Count)
        {
            for (int i = 0; i < _waveList[i].enemyList.Count; i++)              // pass in an enemy count number to determine length of routine 
            {
                ObjectPoolManager.Instance.RequestEnemy();          // Keep requesting a new enemy every iteration.
                yield return _spawnWaitTime;                        // Wait 2 seconds
            }

            WaveCount++;
            Debug.Log("Wave Count: " + WaveCount);
        }

        _enemySpawnRoutine = null;                              // When loop is finished, set the coroutine variable to null so we can run it again.
    }
}
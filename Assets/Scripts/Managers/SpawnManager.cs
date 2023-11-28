using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    private int _waveCount;

    Coroutine _enemySpawnRoutine;                               // Stores a coroutine so only 1 can play.
    WaitForSeconds _spawnWaitTime = new WaitForSeconds(2);      // Caching the wait time between enemies spawning.

    // TODO: Move spawn point back inside room when project is finished
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _endPoint;

    [SerializeField] private List<SOEnemyWaves> _waveList;      // Contains a list of all the SO wave assets.

    #region
    public int WaveCount { get { return _waveCount; } private set { _waveCount = value; } }                  // Counter to control the waves.
    public Transform EndPoint { get { return _endPoint; } }
    public Transform SpawnPoint { get { return _spawnPoint; } }
    #endregion


    void Start()
    {
        WaveCount = 0;
        if (_enemySpawnRoutine == null)
            _enemySpawnRoutine = StartCoroutine(ActivateEnemiesRoutine(0, _waveList[0].enemyList.Count));      // Start the game at wave 1 (element 0)
    }

    //private void StartNewWave(int currentWaveNumber, int enemyCountPerWave)
    //{
    //    if (_enemySpawnRoutine == null)
    //        _enemySpawnRoutine = StartCoroutine(ActivateEnemiesRoutine(currentWaveNumber, _waveList[0].enemyList.Count));
    //}

    // Activate enemies from the ObjectPool

    IEnumerator ActivateEnemiesRoutine(int currentWaveNumber, int enemyCountPerWave)
    {
        // TODO: Update this timer to be a parameter based on the wave number
        yield return new WaitForSeconds(2f);

        int enemyIncrementer = 0;       // Increments until total enemy count in current wave reached 

        while (enemyIncrementer < enemyCountPerWave)     
        {
            GameObject temp = _waveList[currentWaveNumber].enemyList[enemyIncrementer].gameObject;      
            EnemyObjectPool.Instance.RequestEnemy(temp);          
            yield return _spawnWaitTime;
            enemyIncrementer++;
        }

        // Start a new timer to wait until the next wave spawns
        _enemySpawnRoutine = null;                             
    }
}
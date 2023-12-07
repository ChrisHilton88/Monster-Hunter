using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsible for managing and spawning new enemies 
public class SpawnManager : MonoSingleton<SpawnManager>
{
    public static int globalInternalEnemyCount = 0;     

    private int _initialRound;
    [SerializeField] private int _waveCount;
    [SerializeField] private int _totalEnemyCount;
    [SerializeField] private int _currentEnemyCount;

    private float _enemyIntervalTimer;

    Coroutine _enemySpawnRoutine;                               // Stores a coroutine so only 1 can play.

    // TODO: Move spawn point back inside room when project is finished
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _endPoint;

    [SerializeField] private List<SOEnemyWaves> _waveList;      // Contains a list of all the SO wave assets.

    #region
    public int InitialRound { get { return _initialRound; } private set { _initialRound = value; } }    
    public int WaveCount { get { return _waveCount; } private set { _waveCount = value; } }                  // Incrementer to control the waves.
    public int TotalEnemyCount { get { return _totalEnemyCount; } private set { _totalEnemyCount = value; } }               // Keeps track of enemies per round
    public int CurrentEnemyCountInWave { get { return _currentEnemyCount; } private set { _currentEnemyCount = value; } }         // Keeps track of current enemy count
    public float EnemyIntervalTimer { get { return _enemyIntervalTimer; } private set { _enemyIntervalTimer = value; } }
    public Transform EndPoint { get { return _endPoint; } }
    public Transform SpawnPoint { get { return _spawnPoint; } }
    public List<SOEnemyWaves> WaveList { get { return _waveList; } }
    #endregion

    

    #region Initialisation
    private void OnEnable()
    {
        EnemyBase.OnEnemyDeath += TrackRemainingEnemies;
        RoundTimerManager.OnRoundIntermission += RoundIntermission;
        RoundTimerManager.OnRoundStart += StartNewRound;
    }

    private void OnDisable()
    {
        EnemyBase.OnEnemyDeath -= TrackRemainingEnemies;
        RoundTimerManager.OnRoundIntermission -= RoundIntermission;
        RoundTimerManager.OnRoundStart -= StartNewRound;
    }

    private void Start()
    {
        Initialisation();
        if (_enemySpawnRoutine == null)
            _enemySpawnRoutine = StartCoroutine(ActivateEnemiesRoutine(WaveCount, TotalEnemyCount, EnemyIntervalTimer));      // Sets initial values and start round 1
    }

    private void Initialisation()
    {
        InitialRound = 0;
        WaveCount = InitialRound;
        TotalEnemyCount = WaveList[InitialRound].enemyList.Count;
        CurrentEnemyCountInWave = TotalEnemyCount;
        EnemyIntervalTimer = WaveList[InitialRound]._timeIntervalBetweenEnemies;
    }
    #endregion

    #region Methods
    // Responsible for updating global variable incrementer values to a new round 
    private void UpdateInternalRoundValues()
    {
        WaveCount++;
        TotalEnemyCount = WaveList[WaveCount].enemyList.Count;
        CurrentEnemyCountInWave = TotalEnemyCount;
        EnemyIntervalTimer = WaveList[WaveCount]._timeIntervalBetweenEnemies;
    }
    #endregion

    #region Events
    // Event responsible for tracking the internal enemy count per wave
    private void TrackRemainingEnemies()        // Called every time an enemy dies 
    {
        // Check to see if it was the last enemy in a wave
        if (CurrentEnemyCountInWave <= 1)
        {
            RoundTimerManager.OnRoundEnd?.Invoke();     // Trigger event early before timer finishes
            Debug.Log("Invoked OnRoundEnd method - CurrentEnemyCount <= 1");
        }
        else
            CurrentEnemyCountInWave--;
    }

    // Event responsible for updating internal values during intermission between waves
    private void RoundIntermission()
    {
        UpdateInternalRoundValues();
        Debug.Log("Round intermission - Updating internal variables");
    }

    // Event triggered when the intermission timer has come to an end
    private void StartNewRound()
    {
        if (_enemySpawnRoutine == null)
            _enemySpawnRoutine = StartCoroutine(ActivateEnemiesRoutine(WaveCount, TotalEnemyCount, EnemyIntervalTimer));
    }
    #endregion

    #region Coroutines
    private IEnumerator ActivateEnemiesRoutine(int currentWaveNumber, int enemyCountPerWave, float enemyIntervalTimer)
    {
        yield return null;

        int enemyIncrementer = 0;       // Increments until total enemy count in current wave reached, reset each time  

        while (enemyIncrementer < enemyCountPerWave)
        {
            GameObject temp = WaveList[currentWaveNumber].enemyList[enemyIncrementer].gameObject;
            EnemyObjectPool.Instance.RequestEnemy(temp);
            yield return new WaitForSeconds(enemyIntervalTimer);
            enemyIncrementer++;
        }

        _enemySpawnRoutine = null;
    }
    #endregion
}
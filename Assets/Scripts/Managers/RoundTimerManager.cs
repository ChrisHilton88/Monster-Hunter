using UnityEngine;
using System;
using System.Collections;

// Responsible for calculating the time remaining for each round and passing that into the UIManager
public class RoundTimerManager : MonoSingleton<RoundTimerManager>
{
    private double _timer;                    // time tracking for the round
    private float _timerGameOver = 0;         // Set back to 0 when player runs out of time

    private bool _isRoundFinishedEarly;
    private bool _isRoundEnded;

    private Coroutine _roundIntermissionRoutine;

    public static Action OnRoundEnd;            // When the final enemy is killed, trigger event
    public static Action OnRoundIntermission;   // Start a timer at the round end before starting the next wave
    public static Action OnRoundStart;          // Called once the intermission timer is up

    #region Properties
    public double Timer { get { return _timer; } private set { _timer = value; } }
    public bool IsRoundFinishedEarly { get { return _isRoundFinishedEarly; } private set { _isRoundFinishedEarly = value; } }
    public bool IsRoundEnded { get { return _isRoundEnded; } private set { _isRoundEnded = value; } }
    #endregion



    #region Initialisation
    private void OnEnable()
    {
        OnRoundEnd += RoundFinished;
        OnRoundIntermission += RoundIntermission;
    }

    private void OnDisable()
    {
        OnRoundEnd -= RoundFinished;
        OnRoundIntermission -= RoundIntermission;   
    }

    private void Start()
    {
        Timer = SpawnManager.Instance.WaveList[0]._totalRoundTimer;     // set as first round
        _roundIntermissionRoutine = null;   
    }
    #endregion

    #region Methods
    private void Update()
    {
        // If the timer runs out during a round
        if (Timer <= 0 && !IsRoundEnded)       
        {
            OnRoundEnd?.Invoke();
        }
        else if (Timer > 0 && IsRoundEnded)     
        {
            Timer = Timer;
        }
        else if (Timer > 0 && !_isRoundFinishedEarly)
        {
            Timer -= Time.deltaTime;
            Timer = Math.Round(Timer, 2);
            UIManager.Instance.UpdateRemainingTextTime(Timer);
        }
    }
    #endregion

    private void RoundFinished()
    {
        IsRoundEnded = true;

        if (Timer <= 0)
        {
            Timer = 0;
            UIManager.Instance.UpdateRemainingTextTime(Timer);
        }
        else
        {
            UIManager.Instance.UpdateRemainingTextTime(Timer);
            // Apply bonus points
        }

        OnRoundIntermission?.Invoke();
    }

    private void RoundIntermission()
    {
        if (_roundIntermissionRoutine == null)
            _roundIntermissionRoutine = StartCoroutine(RoundIntermissionRoutine());
    }

    // Responsible for simply yielding time between round end and start, allowing internal values/spawns to be updated
    private IEnumerator RoundIntermissionRoutine()
    {
        yield return new WaitForSeconds(3f);

        int roundIncrementer = 0;

        // Update remaining time based on Wave 
        if (roundIncrementer < SpawnManager.Instance.WaveList.Count)
        {
            roundIncrementer++;
            Timer = SpawnManager.Instance.WaveList[roundIncrementer]._totalRoundTimer;
        }
        else
        {
            // Game is over, player wasn't able to kill all the enemies in time.
            // Bring up Game Over menu 
            Timer = _timerGameOver;
        }

        OnRoundStart?.Invoke();     // At the end of the timer start a new wave/round
        Debug.Log("OnRoundStart INVOKED");
        IsRoundEnded = false;
        IsRoundFinishedEarly = false;
        _roundIntermissionRoutine = null;
    }
}

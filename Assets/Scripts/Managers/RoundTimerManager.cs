using UnityEngine;
using System;

// Responsible for calculating the time remaining for each round and passing that into the UIManager
public class RoundTimerManager : MonoSingleton<RoundTimerManager>
{
    // Grab the Round Timer from the scriptable object
    // Display time on TMP
    // Build calculation for reducing the timer

    int roundIncrementer = 0;

    double timer;                    // time tracking for the round
    float timerGameOver = 0;        // Set back to 0 when player runs out of time


    private void Start()
    {
        timer = SpawnManager.Instance.WaveList[0]._totalRoundTimer;
    }

    private void Update()
    {
        if(timer < 0)
        {
            UpdateTimerToNextRound();
        }
        else
        {
            timer -= Time.deltaTime;
            timer = Math.Round(timer, 2);
            UIManager.Instance.UpdateRemainingTextTime(timer);
            Debug.Log("Timer: " + timer);
        }
    }

    private void UpdateTimerToNextRound()
    {
        if (roundIncrementer < SpawnManager.Instance.WaveList.Count)
        {
            roundIncrementer++;
            timer = SpawnManager.Instance.WaveList[roundIncrementer]._totalRoundTimer;
        }
        else
        {
            // Game is over, player wasn't able to kill all the enemies in time.
            // Bring up Game Over menu 
            timer = timerGameOver;
        }
    }
}

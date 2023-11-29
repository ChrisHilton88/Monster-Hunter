// Responsible for handling the points values passed into the Total Score
using UnityEngine;

public class ScoreManager : MonoSingleton<ScoreManager> 
{
    private int _totalScore;


    #region Properties
    public int TotalScore { get { return _totalScore; } set { _totalScore = value; } }
    #endregion


    private void OnEnable()
    {
        EnemyBase.OnEnemyDeath += AddToScore;
    }

    private void AddToScore(int points)
    {
        TotalScore += points;
        Debug.Log("Internal total score: " + TotalScore);
    }
}

using System.Collections.Generic;

// Responsible for handling the points values passed into the Total Score
public class ScoreManager : MonoSingleton<ScoreManager> 
{
    private int _totalScore;


    private Dictionary<int, int> _roundTimerBonusPointsDictionary = new Dictionary<int, int>();        // Currentround, points value
    
    #region Properties
    public int TotalScore { get { return _totalScore; } set { _totalScore = value; } }
    #endregion



    #region Initialisation
    private void Start()
    {
        EndOfRoundPointsBonus();        // Create Dictionary of bonus points
    }
    #endregion

    #region Methods
    // Responsible for calculating bonus points at the end of a round depending on the amount of time left.
    private void CalculateRemainingTimeBonusPoints()
    {
        float _roundBonusMultiplier;

        float currentRoundTimer = SpawnManager.Instance.WaveList[0]._totalRoundTimer;       // Caches the rounds timer
        // Take a snapshot of the time remaining when the player kills the final enemy
        float roundCompletedTimer = 5f;      // When the round is finished, store the time here
        // Calculate difference between time remaining and round time provided
        float timeDifference = currentRoundTimer - roundCompletedTimer;
        // Lookup Dictionary for points guide
        // Display bonus points on the right hand side, similar to floating combat text - labeled 'Round Bonus' 
    }

    // Responsible for storing the points values of each roundat the start of the game
    Dictionary<int, int> EndOfRoundPointsBonus()
    {
        int roundIncremeter = 0;
        int bonusPoints = 100;

        for (int i = 0; i < SpawnManager.Instance.WaveList.Count; i++)
        {
            _roundTimerBonusPointsDictionary.Add(roundIncremeter, bonusPoints);
            roundIncremeter++;
            bonusPoints += 100;
        }

        return _roundTimerBonusPointsDictionary;
    }
    #endregion

    #region Events
    // Responsible for updating the internal score and telling the UIManager
    public void AddPointsToScore(int points)
    {
        TotalScore += points;
        UIManager.Instance.UpdateScoreText();
    }
    #endregion
}

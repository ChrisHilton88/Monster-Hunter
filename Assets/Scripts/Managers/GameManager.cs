using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>   
{
    [SerializeField] private int _totalEnemyCount;

    private bool _gameOver;

    #region Properties
    public int TotalEnemyCount { get { return _totalEnemyCount; } private set { _totalEnemyCount = value; } }
    public bool GameOver { get { return _gameOver; } private set { _gameOver = value; } }

    #endregion



    private void OnEnable()
    {
        EnemyBase.OnEnemyDeath += WinCount;
    }

    private void OnDisable()
    {
        EnemyBase.OnEnemyDeath -= WinCount; 
    }

    void Start()
    {
        CalculateTotalEnemyCount();
        GameOver = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Set Cursor State to None
    public void SetCursorStateToNone()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Responsible for setting the total enemy count for Win Condition
    private int CalculateTotalEnemyCount()
    {
        // Loop over the count of waves and the count of enemies within the waves
        for (int i = 0; i < SpawnManager.Instance.WaveList.Count; i++)
        {
            for (int j = 0; j < SpawnManager.Instance.WaveList[i].enemyList.Count; j++)
            {
                TotalEnemyCount++;
            }
        }

        return TotalEnemyCount;
    }

    // Win Condition decrementer (Win when 0 enemies remain)
    private void WinCount()
    {
        if(TotalEnemyCount <= 0)
        {
            GameOver = true;
            // Win Screen
        }
        else
        {
            TotalEnemyCount--;
        }
    }

    // Sets GameOver state
    public void SetGameOver()
    {
        GameOver = true;
    }

    // Restarts the game.
    public void SetRestartGame()
    {
        SceneManager.LoadScene(0);
    }

    // Closes application.
    void ExitApplication()
    {
        Application.Quit(); 
    }
}

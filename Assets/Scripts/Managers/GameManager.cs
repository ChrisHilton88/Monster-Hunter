using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>   
{
    public bool GameOver { get; private set; }



    void Start()
    {
        GameOver = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Set Cursor State to None
    public void SetCursorStateToNone()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Sets GameOver state.
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

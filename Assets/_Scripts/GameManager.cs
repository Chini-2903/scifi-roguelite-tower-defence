using UnityEngine;
using UnityEngine.SceneManagement; // We need this to reload the level

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver; // Global flag so other scripts know the game ended

    public GameObject gameOverUI;

    void Start()
    {
        GameIsOver = false; // Reset the flag when the game starts
    }

    void Update()
    {
        // If the game is already over, do nothing
        if (GameIsOver)
            return;

        // Check if we ran out of lives
        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        GameIsOver = true;
        Debug.Log("Game Over!");

        // Turn on the Game Over screen
        gameOverUI.SetActive(true);
    }

    // The Retry Button will call this function
    public void Retry()
    {
        // Reloads the exact scene we are currently playing
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to the Game Over UI Panel
    private bool isGameOver = false;

    // Call this function when player health reaches 0
    public void TriggerGameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Invoke("ShowGameOverScreen", 0.5f); // Delay of 0.5 seconds
        }
    }

    private void ShowGameOverScreen()
    {
        gameOverUI.SetActive(true); // Show Game Over UI
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the scene
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Resume time before quitting
        Application.Quit();
    }
}

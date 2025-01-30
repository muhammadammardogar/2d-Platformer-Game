using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryScreenUI; // UI panel for victory
    public GameObject[] uiElementsToHide; // Other UI elements to hide
    public TextMeshProUGUI coinText; // Reference to the UI text showing collected coins

    private bool gameWon = false;
    private int requiredCoins = 5; // Coins needed to win

    private void Start()
    {
        victoryScreenUI.SetActive(false); // Hide victory UI at start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !gameWon && GetCurrentCoinCount() >= requiredCoins)
        {
            ShowVictoryScreen();
        }
    }

    void ShowVictoryScreen()
    {
        gameWon = true;
        victoryScreenUI.SetActive(true); // Show victory UI
        Time.timeScale = 0f; // Pause the game

        // Hide other UI elements
        foreach (GameObject ui in uiElementsToHide)
        {
            ui.SetActive(false);
        }
    }

    int GetCurrentCoinCount()
    {
        return int.Parse(coinText.text); // Get current coins from UI text
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current level
    }

    public void NextLevel()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next level
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit game (only works in build)
        Debug.Log("Game Quit");
    }
}

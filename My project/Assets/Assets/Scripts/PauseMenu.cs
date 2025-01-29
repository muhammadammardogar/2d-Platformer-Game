using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign the Pause Menu Panel
    public GameObject[] uiElementsToHide; // Assign other UI elements that should be hidden when paused
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true); // Show pause menu
        Time.timeScale = 0f; // Pause the game

        // Hide other UI elements
        foreach (GameObject uiElement in uiElementsToHide)
        {
            uiElement.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1f; // Resume the game

        // Show other UI elements again
        foreach (GameObject uiElement in uiElementsToHide)
        {
            uiElement.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Ensure time is reset before quitting
        SceneManager.LoadScene("MainMenu"); // Change "MainMenu" to your actual menu scene name
    }
}

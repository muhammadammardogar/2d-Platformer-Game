using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign the Pause Menu Panel
    public GameObject[] uiElementsToHide; // Assign other UI elements that should be hidden when paused
    public RectTransform cursor; // Assign the cursor image in the Inspector
    public Button[] menuOptions; // Assign all menu buttons (Resume, Volume, Quit) in order
    public TextMeshProUGUI volumeText; // UI text to display volume level

    private bool isPaused = false;
    private int selectedIndex = 0;
    private float volumeLevel = 5; // Default volume level (0-10 scale)

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

        if (isPaused)
        {
            HandleMenuNavigation();
        }
    }

    void HandleMenuNavigation()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuOptions.Length) % menuOptions.Length;
            MoveCursor();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuOptions.Length;
            MoveCursor();
        }

        // Adjust volume when "Volume" option is selected
        if (selectedIndex == 1 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            AdjustVolume(Input.GetKeyDown(KeyCode.A) ? -1 : 1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            menuOptions[selectedIndex].onClick.Invoke(); 
        }
    }

    void MoveCursor()
    {
        Vector3 buttonPosition = menuOptions[selectedIndex].transform.position;
        float offsetX = -315f; 

        cursor.position = new Vector3(buttonPosition.x + offsetX, buttonPosition.y, buttonPosition.z);
    }

    void AdjustVolume(int change)
    {
        volumeLevel = Mathf.Clamp(volumeLevel + change, 0, 10);
        volumeText.text = "Volume: " + volumeLevel;
        AudioListener.volume = volumeLevel / 10f; // Apply volume change to game
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

        MoveCursor(); // Position the cursor on the first option
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
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0); 
    }
}

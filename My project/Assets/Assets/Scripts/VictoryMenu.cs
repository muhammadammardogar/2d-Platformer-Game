using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryScreenUI; // UI panel for victory
    public GameObject[] uiElementsToHide; // Other UI elements to hide
    public TextMeshProUGUI coinText; // Reference to the UI text showing collected coins
    public RectTransform cursor; // Assign the cursor image
    public GameObject[] menuOptions; // Assign all menu options in order (Play Again, Next Level, Quit)

    private bool gameWon = false;
    private int requiredCoins = 5; // Coins needed to win
    private int selectedIndex = 0;

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

        MoveCursor(); // Position cursor on first option
    }

    void Update()
    {
        if (gameWon)
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteSelectedOption();
        }
    }

    void MoveCursor()
    {
        Vector3 optionPosition = menuOptions[selectedIndex].transform.position;
        float offsetX = -315f; // Move cursor to the left of the selected option
        cursor.position = new Vector3(optionPosition.x + offsetX, optionPosition.y, optionPosition.z);
    }

    void ExecuteSelectedOption()
    {
        switch (selectedIndex)
        {
            case 0:
                NextLevel();
                break;
            case 1:
                PlayAgain();
                break;
            case 2:
                QuitGame();
                break;
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
        Time.timeScale = 1f; // Ensure time is reset before quitting
        SceneManager.LoadScene(0);
    }
}

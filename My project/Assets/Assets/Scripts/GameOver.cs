using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to the Game Over UI Panel
    public RectTransform cursor; // Assign the cursor image
    public GameObject[] menuOptions; // Assign all menu options (Restart, Quit) in order

    private bool isGameOver = false;
    private int selectedIndex = 0;

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
        MoveCursor(); // Position cursor on the first option
    }

    void Update()
    {
        if (isGameOver)
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
        float offsetX = -315f; 
        cursor.position = new Vector3(optionPosition.x + offsetX, optionPosition.y, optionPosition.z);
    }

    void ExecuteSelectedOption()
    {
        switch (selectedIndex)
        {
            case 0:
                RestartGame();
                break;
            case 1:
                QuitGame();
                break;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0); 
    }
}

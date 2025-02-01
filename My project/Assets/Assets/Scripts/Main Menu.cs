using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class MainMenu : MonoBehaviour
{
    public Button playButton;    // Reference to the Play button
    public Button quitButton;    // Reference to the Quit button
    public Button musicButton;   // Reference to the Music button
    public AudioSource musicSource; // Reference to the AudioSource component for music
    public TextMeshProUGUI musicButtonText; // Reference to the TextMeshProUGUI for Music button text

    private bool isMusicPlaying = true; // Track if music is currently playing

    void Start()
    {
        // Add listeners for button clicks
        playButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        musicButton.onClick.AddListener(ToggleMusic);

        // Initialize music state based on the current setting
        UpdateMusicButtonText();
    }

    // Start the game by loading the next scene
    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Quit the game
    void QuitGame()
    {
        Application.Quit(); // Quit the game 
        Debug.Log("Game Quit");
    }

    // Toggle the music on/off
    void ToggleMusic()
    {
        isMusicPlaying = !isMusicPlaying; // Toggle the state
        musicSource.mute = !isMusicPlaying; // Mute/unmute the music

        UpdateMusicButtonText(); // Update the button text based on the music state
    }

    // Update the music button text to reflect the current music state
    void UpdateMusicButtonText()
    {
        if (isMusicPlaying)
        {
            musicButtonText.text = "Music: On";
        }
        else
        {
            musicButtonText.text = "Music: Off";
        }
    }
}

using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TextMeshProUGUI healthUIText; // TextMeshPro for health UI

    private int currentHealth;
    private Animator playerAnimator; // Reference to the Animator
    private GameOverManager gameOverManager; // Reference to GameOverManager

    void Start()
    {
        playerAnimator = GetComponent<Animator>(); // Get the Animator component
        gameOverManager = FindObjectOfType<GameOverManager>(); // Find GameOverManager in scene

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        playerAnimator.SetTrigger("Hurt"); // Play Hurt animation

        if (currentHealth <= 0) // Ensure death triggers when health is 0 or below
        {
            Die();
        }
    }
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    private void UpdateHealthUI()
    {
        if (healthUIText != null)
        {
            healthUIText.text = "" + currentHealth; // Update health UI
        }
    }

    private void Die()
    {
        playerAnimator.SetTrigger("Death"); // Play Death animation
        Debug.Log("Player Died");

        // Delay Game Over screen
        Invoke("GameOver", 0.5f);
    }

    private void GameOver()
    {
        gameOverManager.TriggerGameOver(); // Call Game Over
    }
}
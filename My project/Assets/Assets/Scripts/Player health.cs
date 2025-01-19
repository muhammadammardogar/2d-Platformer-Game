using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TextMeshProUGUI healthUIText; // TextMeshPro for health UI

    private int currentHealth;

    private Animator playerAnimator; // Reference to the Animator

    void Start()
    {
        playerAnimator = GetComponent<Animator>(); // Get the Animator component
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        playerAnimator.SetTrigger("Hurt"); // Trigger hurt animation when taking damage

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthUIText != null)
        {
            healthUIText.text = "Health: " + currentHealth; // Update the health UI
        }
    }

    private void Die()
    {
        playerAnimator.SetTrigger("Death"); // Trigger death animation when health reaches 0
        Debug.Log("Player Died");

        // Additional game over logic can go here
    }
}

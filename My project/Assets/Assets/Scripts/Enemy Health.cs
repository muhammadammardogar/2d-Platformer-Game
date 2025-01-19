using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health_enemy = 100;          // Health of the enemy
    public int damage = 10;           // Damage the enemy deals (not used in this script, but can be useful)
    private Animator m_animator;      // Reference to the enemy's animator

    // Start is called before the first frame update
    void Start()
    {
        // Get the animator component attached to the enemy
        m_animator = GetComponent<Animator>();
    }

    // Method to deal damage to the enemy
    public void TakeDamage_enemy(int damageAmount)
    {
        // Decrease the health by the damage amount
        health_enemy -= damageAmount;

        // Play the "Hurt" animation
        m_animator.SetTrigger("Hurt");

        // Check if the enemy's health is less than or equal to zero
        if (health_enemy <= 0)
        {
            // If so, trigger the "Death" animation and destroy the enemy after 1 second
            Die_enemy();
        }
    }

    // Method to handle the enemy's death
    private void Die_enemy()
    {
        // Trigger the "Death" animation
        m_animator.SetTrigger("Death");

        // Destroy the enemy object after 1 second, to allow the death animation to finish
        Destroy(gameObject, 1f);
    }

    // This method handles sword collision detection (called when the sword collider enters the enemy's collider)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is from the sword and the sword is active
        if (other.CompareTag("Sword"))
        {
            // Deal 20 damage to the enemy (you can change the amount as needed)
            TakeDamage_enemy(20);
        }
    }
}

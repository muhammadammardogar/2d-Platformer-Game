using UnityEngine;

public class Sword : MonoBehaviour
{
    private int attackDamage;

    public void SetDamage(int damage)
    {
        attackDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth?.TakeDamage(attackDamage);
        }
    }
}
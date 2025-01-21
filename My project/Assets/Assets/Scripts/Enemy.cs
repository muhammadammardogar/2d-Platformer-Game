using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public int maxHealth = 100;
    public int damage = 10;
    public float leftBoundary; // Define in the Inspector
    public float rightBoundary; // Define in the Inspector

    private int currentHealth;
    private Transform player;
    private Animator animator;
    private bool isMovingRight = true; // Determines patrol direction
    private bool isRoaming = true;
    private bool isAttacking = false;
    private Collider2D swordCollider;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        // Initialize health
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            FollowPlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (isRoaming)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        animator.SetBool("IsWalking", true);

        // Move in the current direction
        if (isMovingRight)
        {
            transform.Translate(Vector2.right * roamSpeed * Time.deltaTime);

            // Reverse direction if at the right boundary
            if (transform.position.x >= rightBoundary)
            {
                isMovingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * roamSpeed * Time.deltaTime);

            // Reverse direction if at the left boundary
            if (transform.position.x <= leftBoundary)
            {
                isMovingRight = true;
                Flip();
            }
        }
    }

    private void FollowPlayer()
    {
        isRoaming = false;
        animator.SetBool("IsWalking", true);

        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * roamSpeed * Time.deltaTime);

        // Flip based on player position
        if (player.position.x > transform.position.x && !isMovingRight)
        {
            isMovingRight = true;
            Flip();
        }
        else if (player.position.x < transform.position.x && isMovingRight)
        {
            isMovingRight = false;
            Flip();
        }
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            // Randomly choose an attack
            bool useFirstAttack = Random.value > 0.5f;
            if (useFirstAttack)
            {
                animator.SetBool("Attack1", true);
                animator.SetBool("Attack2", false);
            }
            else
            {
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack2", true);
            }

            // Enable sword collider during the attack
            EnableSwordCollider();

            Invoke(nameof(ResetAttack), 1.5f); // Delay to reset attack
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
    }

    private void EnableSwordCollider()
    {
        GameObject sword = transform.Find("Sword")?.gameObject;
        if (sword == null)
        {
            Debug.LogError("Sword object not found!");
            return;
        }

        swordCollider = sword.GetComponent<Collider2D>();
        if (swordCollider == null)
        {
            Debug.LogError("Sword Collider2D not found!");
            return;
        }

        swordCollider.enabled = true;
    }

    private void DisableSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die"); // Trigger the death animation
        Destroy(gameObject, 1f);    // Destroy the enemy after the animation
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Debug Log to check if sword collides with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (swordCollider != null && swordCollider.enabled)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Sword collided with player! Inflicting damage.");
                player.GetComponent<PlayerHealth>()?.TakeDamage(damage); // Apply damage to the player
            }
        }
    }
}

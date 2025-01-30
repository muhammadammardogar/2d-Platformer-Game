using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public int maxHealth = 100;
    public int damage = 10;
    public float leftBoundary;
    public float rightBoundary;

    private int currentHealth;
    private Transform player;
    private PlayerHealth playerHealth;
    private Animator animator;
    private bool isMovingRight = true;
    private bool isRoaming = true;
    private bool isAttacking = false;
    private Collider2D swordCollider;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource attackSound; //  Attack sound

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the correct tag.");
        }

        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Stop enemy actions if player is dead
        if (playerHealth == null || playerHealth.IsDead())
        {
            StopAllActions();
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

        if (isMovingRight)
        {
            transform.Translate(Vector2.right * roamSpeed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                isMovingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * roamSpeed * Time.deltaTime);
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

            // Stop attack if player is dead
            if (playerHealth.IsDead())
            {
                StopAllActions();
                return;
            }

            bool useFirstAttack = Random.value > 0.5f;
            animator.SetBool("Attack1", useFirstAttack);
            animator.SetBool("Attack2", !useFirstAttack);

            EnableSwordCollider();

            // Play the attack sound 
            if (attackSound != null && !attackSound.isPlaying)
            {
                attackSound.Play();
            }

            Invoke(nameof(ResetAttack), 1.5f);
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
        if (sword == null) return;

        swordCollider = sword.GetComponent<Collider2D>();
        if (swordCollider != null) swordCollider.enabled = true;
    }

    private void DisableSwordCollider()
    {
        if (swordCollider != null) swordCollider.enabled = false;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 1f);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void StopAllActions()
    {
        isAttacking = false;
        isRoaming = false;
        animator.SetBool("IsWalking", false);
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (swordCollider != null && swordCollider.enabled && other.CompareTag("Player"))
        {
            playerHealth?.TakeDamage(damage);
        }
    }
}

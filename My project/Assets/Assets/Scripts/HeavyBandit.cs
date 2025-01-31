using UnityEngine;

public class HeavyBandit : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRadius = 1.5f;
    public int maxHealth = 100;
    public int damage = 10;
    public float leftBoundary;
    public float rightBoundary;
    public LayerMask attackLayerMask;

    private int currentHealth;
    private Transform player;
    private PlayerHealth playerHealth;
    private Animator animator;
    private bool isMovingRight = true;
    private bool isRoaming = true;
    private bool isAttacking = false;
    public Transform attackPoint;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource attackSound;

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

        if (playerHealth == null || playerHealth.IsDead())
        {
            StopAllActions();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRadius)
            {
                FollowPlayer();
            }
            else if (distanceToPlayer <= attackRadius)
            {
                AttackPlayer();
            }
            else if (isRoaming)
            {
                Patrol();
            }
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
            animator.SetBool("Attack1", true);
            animator.SetBool("IsWalking", false);

            if (attackSound != null && !attackSound.isPlaying)
            {
                attackSound.Play();
            }

            Invoke(nameof(PerformAttack), 0.5f);
            Invoke(nameof(ResetAttack), 1.5f);
        }
    }

    private void PerformAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, attackLayerMask);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player"))
            {
                enemy.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack1", false);
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
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
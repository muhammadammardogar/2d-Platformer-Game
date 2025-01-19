using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public int health = 100;
    public int damage = 10;

    private Transform player;
    private Animator animator;
    private Vector2 roamDirection;
    private bool isRoaming = true;
    private bool isAttacking = false;
    private Collider2D swordCollider;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        roamDirection = GetRandomDirection();
        InvokeRepeating(nameof(ChangeRoamDirection), 3f, 3f);
    }

    private void Update()
    {
        if (health <= 0)
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
            Roam();
        }
    }

    private void Roam()
    {
        isRoaming = true;
        animator.SetBool("IsWalking", true); // Keep walking animation when roaming
        transform.Translate(roamDirection * roamSpeed * Time.deltaTime);

        // Reverse direction if hitting a wall
        RaycastHit2D hit = Physics2D.Raycast(transform.position, roamDirection, 0.5f);
        if (hit.collider != null)
        {
            roamDirection = GetRandomDirection();
        }
    }

    private void FollowPlayer()
    {
        isRoaming = false;
        animator.SetBool("IsWalking", true);
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * roamSpeed * Time.deltaTime);
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
        GameObject sword = transform.Find("Sword").gameObject; // Adjust if necessary
        if (sword != null)
        {
            Collider2D swordCollider = sword.GetComponent<Collider2D>();
            if (swordCollider != null)
            {
                swordCollider.enabled = false;
            }
        }
    }

    private void ChangeRoamDirection()
    {
        roamDirection = GetRandomDirection();
    }

    private Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die"); // Add a death state if needed
        Destroy(gameObject, 1f); // Destroy after death animation plays
    }

    // Debug Log to check if sword collides with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (swordCollider != null && swordCollider.enabled)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Sword collided with player! Inflicting damage.");
                player.GetComponent<PlayerHealth>()?.TakeDamage(damage);  // Apply damage to the player
            }
        }
    }
}
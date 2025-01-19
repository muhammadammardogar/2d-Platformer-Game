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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Ensure your player has the tag "Player"
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
        animator.Play("Skeleton Walking"); // Ensure this plays your walking animation
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
        animator.Play("Skeleton Walking"); // Keep walking animation while following the player
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

            // Assuming Player has a script with a method `TakeDamage(int amount)`
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);

            Invoke(nameof(ResetAttack), 1.5f); // Delay to reset attack
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
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
}

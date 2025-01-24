using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed of enemy movement
    public float jumpForce = 5f; // Force applied for jumping
    public Transform leftLimit; // Left patrol limit
    public Transform rightLimit; // Right patrol limit
    public float detectionRange = 10f; // Range to detect the player
    public float attackRange = 1f; // Range to attack the player
    public float minimumDistance = 2f; // Minimum distance to maintain from the player
    public float attackCooldown = 1.5f; // Time between attacks
    public int damage = 10; // Damage dealt to the player
    public float collisionDamageCooldown = 2f; // Time between consecutive collision damage

    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform firePoint; // Point where bullets are fired from
    public float bulletSpeed = 5f; // Speed of the bullet

    private Transform player; // Reference to the player
    private float attackTimer; // Timer for attack cooldown
    private bool isGrounded = false; // To check if the enemy is on the ground
    private float collisionDamageTimer = 0f; // Timer for collision damage cooldown

    private Animator animator; // Optional: To trigger enemy animations
    private Rigidbody2D rb;

    public LayerMask groundLayer; // Layer for detecting the ground
    public Transform groundCheck; // Position to check for the ground
    public float groundCheckRadius = 0.2f; // Radius for ground detection
    public float horizontalFlipThreshold = 0.5f; // Minimum horizontal distance to trigger a flip

    private bool isFlipped = false; // Track if the enemy is currently flipped

    public AudioSource jumpp;

    private void Start()
    {
        attackTimer = 1.5f;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find player by tag
        animator = GetComponent<Animator>(); // If the enemy uses animations
    }

    private void Update()
    {
        // Check if the enemy is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Decrement collision damage timer
        if (collisionDamageTimer > 0)
        {
            collisionDamageTimer -= Time.deltaTime;
        }

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer(distanceToPlayer);
            ShootAtPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (transform.position.x <= leftLimit.position.x)
        {
            Flip(false); // Face right
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
        else if (transform.position.x >= rightLimit.position.x)
        {
            Flip(true); // Face left
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(movingRight() ? moveSpeed : -moveSpeed, rb.linearVelocity.y);
        }

        // Optional: Trigger patrol animation
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        // Stop moving if within the minimum distance
        if (distanceToPlayer > minimumDistance)
        {
            // Move toward the player
            if (player.position.x > transform.position.x + horizontalFlipThreshold)
            {
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
                Flip(true); // Face right
            }
            else if (player.position.x < transform.position.x - horizontalFlipThreshold)
            {
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
                Flip(false); // Face left
            }

            // Jump if the player is on a higher platform and the enemy is grounded
            if (isGrounded && player.position.y > transform.position.y + 0.5f)
            {
                Jump();
            }

            // Optional: Trigger chase animation
            if (animator != null)
            {
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            // Stop moving if within the minimum distance
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            // Optional: Stop walking animation
            if (animator != null)
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpp.Play();
        // Optional: Trigger jump animation
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero; // Stop moving to attack

        if (attackTimer <= 0)
        {
            // Optional: Trigger attack animation
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            Debug.Log("Enemy attacks player and deals " + damage + " damage!");
            // You can call a method to reduce player health here.

            attackTimer = attackCooldown; // Reset the cooldown
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }

        // Optional: Stop movement animation
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void ShootAtPlayer()
    {
        if (attackTimer <= 0)
        {
            // Instantiate a bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            // Calculate direction toward the player
            Vector2 direction = (player.position - firePoint.position).normalized;

            // Set bullet velocity
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = direction * bulletSpeed;

            // Reset the attack cooldown
            attackTimer = attackCooldown;

            // Optional: Trigger shooting animation
            if (animator != null)
            {
                animator.SetTrigger("Shoot");
            }

            Debug.Log("Enemy shoots at the player!");
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void Flip(bool faceLeft)
    {
        Vector3 localScale = transform.localScale;

        // Only flip if the current direction doesn't match the target direction
        if (faceLeft && !isFlipped)
        {
            localScale.x = -Mathf.Abs(localScale.x); // Face left
            transform.localScale = localScale;
            isFlipped = true;
        }
        else if (!faceLeft && isFlipped)
        {
            localScale.x = Mathf.Abs(localScale.x); // Face right
            transform.localScale = localScale;
            isFlipped = false;
        }
    }

    private bool movingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collisionDamageTimer <= 0)
        {
            // Get the PlayerHealth component and apply damage
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player takes " + damage + " damage from collision!");
            }

            // Reset the collision damage timer
            collisionDamageTimer = collisionDamageCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw patrol limits
        Gizmos.color = Color.green;
        Gizmos.DrawLine(leftLimit.position, rightLimit.position);

        // Draw detection and attack ranges
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw minimum distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minimumDistance);

        // Draw ground check
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}

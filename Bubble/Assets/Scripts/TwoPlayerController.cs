using UnityEngine;
using UnityEngine.EventSystems;

public class TwoPlayerController : MonoBehaviour
{
    [Header("Player 1 Settings")]
    public GameObject player1; // Player 1 GameObject
    public float player1MoveSpeed = 5f; // Speed for Player 1
    public float player1JumpForce = 7f; // Jump force for Player 1
    public KeyCode player1Left = KeyCode.A; // Move left key for Player 1
    public KeyCode player1Right = KeyCode.D; // Move right key for Player 1
    public KeyCode player1Jump = KeyCode.W; // Jump key for Player 1
    public KeyCode player1Attack = KeyCode.Space; // Attack key for Player 1
    public float player1BulletSpeed = 10f; // Speed of Player 1's bullets

    [Header("Player 2 Settings")]
    public GameObject player2; // Player 2 GameObject
    public float player2MoveSpeed = 5f; // Speed for Player 2
    public float player2JumpForce = 7f; // Jump force for Player 2
    public KeyCode player2Left = KeyCode.LeftArrow; // Move left key for Player 2
    public KeyCode player2Right = KeyCode.RightArrow; // Move right key for Player 2
    public KeyCode player2Jump = KeyCode.UpArrow; // Jump key for Player 2
    public float player2BulletSpeed = 10f; // Speed of Player 2's bullets

    [Header("Attack Settings")]
    public GameObject player1BulletPrefab; // Prefab for Player 1's bullet
    public Transform player1BulletSpawnPoint; // Spawn point for Player 1's bullet
    public GameObject player2BulletPrefab; // Prefab for Player 2's bullet
    public Transform player2BulletSpawnPoint; // Spawn point for Player 2's bullet

    [Header("Sound Effects")]
    public AudioClip player1JumpSound;  // Jump sound for Player 1
    public AudioClip player1AttackSound; // Attack sound for Player 1
    public AudioClip player2JumpSound;  // Jump sound for Player 2
    public AudioClip player2AttackSound; // Attack sound for Player 2

    private Rigidbody2D rb1; // Rigidbody for Player 1
    private Rigidbody2D rb2; // Rigidbody for Player 2
    private bool isGrounded1 = true; // Is Player 1 on the ground?
    private bool isGrounded2 = true; // Is Player 2 on the ground?

    private AudioSource player1AudioSource; // AudioSource for Player 1
    private AudioSource player2AudioSource; // AudioSource for Player 2

    public LayerMask groundLayer; // Layer to check for ground
    public Transform groundCheck1; // Ground check for Player 1
    public Transform groundCheck2; // Ground check for Player 2
    public float groundCheckRadius = 0.2f; // Radius for ground check
    bool IsPointerOverPauseTag()
    {
        // Check if pointer is over any UI element with "Pause" tag
        return EventSystem.current.IsPointerOverGameObject() &&
               EventSystem.current.currentSelectedGameObject != null &&
               EventSystem.current.currentSelectedGameObject.CompareTag("Pause");
    }
    void Start()
    {
        rb1 = player1.GetComponent<Rigidbody2D>();
        rb2 = player2.GetComponent<Rigidbody2D>();

        player1AudioSource = player1.GetComponent<AudioSource>();
        player2AudioSource = player2.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Player 1 controls
        HandlePlayer1Movement();
        HandlePlayer1Attack();

        // Player 2 controls
        HandlePlayer2Movement();
        HandlePlayer2Attack();
    }

    private void HandlePlayer1Movement()
    {
        // Horizontal movement
        float moveDirection = 0f;
        if (Input.GetKey(player1Left)) moveDirection = -1f;
        if (Input.GetKey(player1Right)) moveDirection = 1f;

        rb1.linearVelocity = new Vector2(moveDirection * player1MoveSpeed, rb1.linearVelocity.y);

        // Flip the player based on movement direction
        if (moveDirection < 0)
            player1.transform.localScale = new Vector3(-1.8f, 1.8f, 1f); // Face left
        else if (moveDirection > 0)
            player1.transform.localScale = new Vector3(1.8f, 1.8f, 1f); // Face right

        // Jump
        isGrounded1 = Physics2D.OverlapCircle(groundCheck1.position, groundCheckRadius, groundLayer);
        if (isGrounded1 && Input.GetKeyDown(player1Jump))
        {
            rb1.linearVelocity = new Vector2(rb1.linearVelocity.x, player1JumpForce);
            if (player1JumpSound != null)
            {
                player1AudioSource.PlayOneShot(player1JumpSound);  // Play jump sound
            }
        }
    }

    private void HandlePlayer1Attack()
    {
        if (Input.GetKeyDown(player1Attack))
        {
            // Instantiate Player 1's bullet
            GameObject bullet = Instantiate(player1BulletPrefab, player1BulletSpawnPoint.position, Quaternion.identity);

            // Set bullet velocity
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = new Vector2(player1BulletSpeed * GetPlayerDirection(player1), 0f);

            // Optional: Destroy the bullet after some time
            Destroy(bullet, 3f);

            if (player1AttackSound != null)
            {
                player1AudioSource.PlayOneShot(player1AttackSound);  // Play attack sound
            }
        }
    }

    private void HandlePlayer2Movement()
    {
        // Horizontal movement
        float moveDirection = 0f;
        if (Input.GetKey(player2Left)) moveDirection = -1f;
        if (Input.GetKey(player2Right)) moveDirection = 1f;

        rb2.linearVelocity = new Vector2(moveDirection * player2MoveSpeed, rb2.linearVelocity.y);

        // Flip the player based on movement direction
        if (moveDirection < 0)
            player2.transform.localScale = new Vector3(-1.8f, 1.8f, 1f); // Face left
        else if (moveDirection > 0)
            player2.transform.localScale = new Vector3(1.8f, 1.8f, 1f); // Face right

        // Jump
        isGrounded2 = Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, groundLayer);
        if (isGrounded2 && Input.GetKeyDown(player2Jump))
        {
            rb2.linearVelocity = new Vector2(rb2.linearVelocity.x, player2JumpForce);
            if (player2JumpSound != null)
            {
                player2AudioSource.PlayOneShot(player2JumpSound);  // Play jump sound
            }
        }
    }

    private void HandlePlayer2Attack()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverPauseTag()) // Left mouse button
        {
            // Instantiate Player 2's bullet
            GameObject bullet = Instantiate(player2BulletPrefab, player2BulletSpawnPoint.position, Quaternion.identity);

            // Set bullet velocity
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = new Vector2(player2BulletSpeed * -GetPlayerDirection(player2), 0f);

            // Optional: Destroy the bullet after some time
            Destroy(bullet, 3f);

            if (player2AttackSound != null)
            {
                player2AudioSource.PlayOneShot(player2AttackSound);  // Play attack sound
            }
        }
    }

    private int GetPlayerDirection(GameObject player)
    {
        // Determine the direction the player is facing
        return player.transform.localScale.x > 0 ? 1 : -1;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw ground check for Player 1
        Gizmos.color = Color.green;
        if (groundCheck1 != null)
        {
            Gizmos.DrawWireSphere(groundCheck1.position, groundCheckRadius);
        }

        // Draw ground check for Player 2
        Gizmos.color = Color.blue;
        if (groundCheck2 != null)
        {
            Gizmos.DrawWireSphere(groundCheck2.position, groundCheckRadius);
        }
    }
}

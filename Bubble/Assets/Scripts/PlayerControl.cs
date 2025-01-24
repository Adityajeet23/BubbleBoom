using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.2f);
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public AudioSource jummpp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();

        // Check if not over a UI element with "Pause" tag before firing
        if (Input.GetMouseButtonDown(0) && !IsPointerOverPauseTag())
        {
            Fire();
        }
    }

    bool IsPointerOverPauseTag()
    {
        // Check if pointer is over any UI element with "Pause" tag
        return EventSystem.current.IsPointerOverGameObject() &&
               EventSystem.current.currentSelectedGameObject != null &&
               EventSystem.current.currentSelectedGameObject.CompareTag("Pause");
    }

    // Rest of the script remains the same as your original...
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f);
        }
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jummpp.Play();
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        float bulletDirection = transform.localScale.x > 0 ? 1 : -1;
        bulletRb.linearVelocity = new Vector2(bulletDirection * bulletSpeed, 0);

        Destroy(bullet, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }

        if (firePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
        }
    }
}
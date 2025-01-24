using System.Collections;
using UnityEngine;

public class BulletCollision2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource fade;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(10);
        }
        rb.simulated=false;
        animator.SetTrigger("PoP");
        // Destroy the bullet when it collides with any surface
        StartCoroutine(delaydestroy());
    }
    IEnumerator delaydestroy()
    {
        fade.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}   

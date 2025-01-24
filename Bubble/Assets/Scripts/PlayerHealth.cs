using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject blast;
    public int maxHealth = 100;
    private int currentHealth;
    public Slider Health;
    public Animator animator;
    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
        animator = GetComponent<Animator>();    
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
        Health.value = (float)currentHealth/(float)maxHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject);
        Instantiate(blast,transform.position,Quaternion.identity);
        
        // Handle player death (e.g., reload level or show game over screen)
    }
}

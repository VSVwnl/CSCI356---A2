using UnityEngine;

public class dmgoncreep : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 100; // Maximum health
    private int currentHealth; // Current health

    [Header("Death Effects")]
    [SerializeField] private GameObject deathEffect; // Particle effect or animation when the enemy dies

    // Start is called before the first frame update
    private void Start()
    {
        // Set the enemy's health to maximum at the start
        currentHealth = maxHealth;
    }

    // This function will be called when the enemy is hit by a projectile or raycast
    public void TakeDamage(int damage)
    {
        // Subtract damage from current health
        currentHealth -= damage;

        // If health drops to or below zero, trigger death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function to handle enemy death
    private void Die()
    {
        // Play death effect, if available
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }

    // Optional: Add some debug to test if the enemy is taking damage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Assuming the projectile does 20 damage
            TakeDamage(20);

            // Destroy the projectile on collision
            Destroy(collision.gameObject);
        }
    }
}

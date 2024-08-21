using UnityEngine;

public class shootThis : MonoBehaviour
{
    [Tooltip("Health of the enemy.")]
    [SerializeField]
    private int health = 100;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle enemy death (e.g., play animation, drop loot, etc.)
        Destroy(gameObject);
    }
}
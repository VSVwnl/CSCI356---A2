using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    private void Start()
    {
        // Get the EnemyHealth component from the same GameObject
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            Debug.LogError("No EnemyHealth component found on this GameObject.");
        }
    }

    public void ApplyDamage(int damage)
    {
        if (enemyHealth != null)
        {
            // Reduce the enemy's health using the EnemyHealth script
            enemyHealth.TakeDamage(damage);

            // Check if the enemy is dead
            if (enemyHealth.health <= 0)
            {
                Debug.Log(gameObject.name + " is dead.");
                // Handle the enemy's death (e.g., deactivate the object or trigger death animation)
                gameObject.SetActive(false); 
            }
        }
    }
}
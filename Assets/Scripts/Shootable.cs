using System;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Enemy enemy;
    private MeleeEnemy meleeEnemy;

    private void Start()
    {
        // Get the EnemyHealth and Enemy components from the same GameObject
        enemyHealth = GetComponent<EnemyHealth>();
        enemy = GetComponent<Enemy>();

        if (enemyHealth == null)
        {
            Debug.LogError("No EnemyHealth component found on this GameObject.");
        }

        // Check if the enemy is of type MeleeEnemy
        meleeEnemy = GetComponent<MeleeEnemy>();
        if (meleeEnemy != null)
        {
            Debug.Log("MeleeEnemy component found.");
        }
    }

    public void ApplyDamage(int damage)
    {
        if (enemyHealth != null)
        {
            // Apply damage to the health component
            enemyHealth.TakeDamage(damage);

            // Check if the enemy is of type MeleeEnemy
            if (meleeEnemy != null)
            {
                // Handle MeleeEnemy-specific behavior
                Debug.Log("Applying damage to MeleeEnemy");
                meleeEnemy.TakeDamage(damage); // Call MeleeEnemy's TakeDamage method
            }
            else if (enemy != null)
            {
                // Handle other types of enemies
                Debug.Log("Applying damage to Enemy");
                enemy.TakeDamage(damage); // Call Enemy's TakeDamage method
            }

            // If the enemy's health reaches zero, trigger death
            if (enemyHealth.health <= 0)
            {
                // Ensure death is handled only once
                if (meleeEnemy != null && !meleeEnemy.isDead)
                {
                    meleeEnemy.Die(); // Calls the Die() method in the MeleeEnemy script to handle death logic
                }
                else if (enemy != null && !enemy.isDead)
                {
                    enemy.Die(); // Calls the Die() method in the Enemy script to handle death logic
                }
            }
        }
    }

    internal int GetHealth()
    {
        throw new NotImplementedException();
    }
}
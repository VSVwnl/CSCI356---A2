using System;
using System.Collections;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Enemy enemy;

    private void Start()
    {
        // Get the EnemyHealth and Enemy components from the same GameObject
        enemyHealth = GetComponent<EnemyHealth>();
        enemy = GetComponent<Enemy>();

        if (enemyHealth == null)
        {
            Debug.LogError("No EnemyHealth component found on this GameObject.");
        }
    }

    public void ApplyDamage(int damage)
    {
        if (enemyHealth != null)
        {
            // Apply damage and trigger hit reactions
            enemyHealth.TakeDamage(damage);

            if (enemy != null)
            {
                // Trigger the hit animation and damage reaction
                enemy.TakeDamage(damage);
            }

            // If the enemy's health reaches zero, trigger death
            if (enemyHealth.health <= 0)
            {
                // Ensure death is handled only once
                if (!enemy.isDead)
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

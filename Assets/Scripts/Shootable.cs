using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Enemy enemy;

    private void Start()
    {
        // Get the EnemyHealth component from the same GameObject
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
            // Reduce the enemy's health using the EnemyHealth script
            enemyHealth.TakeDamage(damage);

            // Trigger the hit animation
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Check if the enemy is dead
            if (enemyHealth.health <= 0)
            {
                StartCoroutine(DeactivateAfterReset()); // Coroutine to deactivate after resetting
            }
        }
    }

    private IEnumerator DeactivateAfterReset()
    {
        // Wait until the hit state has been reset
        yield return new WaitUntil(() => !enemy.isHit);

        // Now it's safe to deactivate the GameObject
        gameObject.SetActive(false);
    }
}
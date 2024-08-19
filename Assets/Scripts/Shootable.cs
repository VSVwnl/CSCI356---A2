using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Animator animator;

    private void Start()
    {
        // Get the EnemyHealth component from the same GameObject
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();

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

            // Trigger hit animation
            if (animator != null)
            {
                animator.SetBool("isHit", true);
            }

            // Check if the enemy is dead
            if (enemyHealth.health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
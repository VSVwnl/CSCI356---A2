using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI EnemyHealthText;
    public Animator animator; // Reference to the Animator
    public GameObject gun;

    private bool isDead = false;

    void Start()
    {
        health = maxHealth;
        UpdateHealthUI(); // Initialize the UI at the start
    }

    // Call this when the enemy takes damage
    public void TakeDamage(float damage)
    {
        if (isDead) return; // Do nothing if already dead

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Clamp health between 0 and maxHealth

        UpdateHealthUI(); // Immediately update the health bar

        // Check if the enemy is dead
        if (health <= 0)
        {
            Die(); // Trigger death if health reaches 0
        }
    }

    // Updates the health bar and text UI
    void UpdateHealthUI()
    {
        if (frontHealthBar != null && backHealthBar != null && EnemyHealthText != null)
        {
            float hFraction = health / maxHealth;

            // Update the health bar fill amount
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.fillAmount = hFraction;

            // Update the health text
            EnemyHealthText.text = Mathf.RoundToInt(health) + "/" + Mathf.RoundToInt(maxHealth);
        }
        else
        {
            Debug.LogWarning("One or more UI elements are not assigned.");
        }
    }

    // Handles the death of the enemy
    void Die()
    {
        if (isDead) return; // Prevent multiple death triggers

        isDead = true;
        Debug.Log("Enemy Died!");

         if (animator != null)
        {
            animator.SetBool("isDead", true); // Set the isDead boolean to true
        }

        if (gun != null)
        {
            Destroy(gun);
        }

        // Start coroutine to deactivate after 3 minutes
        StartCoroutine(DeactivateAfterDelay());
    }

    // Coroutine to deactivate the enemy after 3 minutes
    IEnumerator DeactivateAfterDelay()
    {
        // Wait for 3 minutes (180 seconds)
        yield return new WaitForSeconds(10f);

        // Deactivate the GameObject
        gameObject.SetActive(false);
    }
}
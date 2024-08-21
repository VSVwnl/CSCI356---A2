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
            animator.SetTrigger("Death");
        }

        // Deactivate the enemy after the death animation finishes
        StartCoroutine(DeactivateAfterAnimation());
    }

    // Coroutine to deactivate the enemy after the death animation
    IEnumerator DeactivateAfterAnimation()
    {
        // Get the current animation state information
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait until the death animation finishes
        yield return new WaitForSeconds(stateInfo.length);

        // Deactivate the GameObject after the animation finishes
        gameObject.SetActive(false);
    }
}

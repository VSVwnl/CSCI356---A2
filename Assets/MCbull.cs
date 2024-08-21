using UnityEngine;

public class MCBull : MonoBehaviour
{
    [Tooltip("Damage dealt by the bullet.")]
    [SerializeField]
    private int damage = 10;

    [Tooltip("Layers that the bullet should interact with.")]
    [SerializeField]
    private LayerMask collisionMask;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit is on the specified collision mask.
        if ((collisionMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            // Try to get the Enemy component from the hit object.
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                // If the object has an Enemy component, apply damage.
                enemy.TakeDamage(damage);
            }

            // Destroy the bullet after it hits something.
            Destroy(gameObject);
        }
    }

    // Alternatively, you can use OnTriggerEnter if your bullet and enemies use triggers.
    private void OnTriggerEnter(Collider other)
    {
        if ((collisionMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
using UnityEngine;

// Handles health for player or enemy.
public class Health : MonoBehaviour
{
    public int maxHealth = 5;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Call this to deal damage
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // For now, just destroy the object
        Destroy(gameObject);
    }
}
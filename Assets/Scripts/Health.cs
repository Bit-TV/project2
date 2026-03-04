using System.Collections;
using UnityEngine;

// Simple health component for anything that can take damage and die.
public class Health : MonoBehaviour
{
    public int maxHealth = 5;

    private int currentHealth;

    // Used for the damage flash effect
    private SpriteRenderer sr;
    private Coroutine flashRoutine;

    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        // Try to find a SpriteRenderer on this object
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Visual feedback: flash the sprite if it exists
        if (sr != null)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(DamageFlash());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        // Save the original color
        Color original = sr.color;

        // Flash to white (or red if you prefer)
        sr.color = Color.white;

        // Wait a short time
        yield return new WaitForSeconds(0.08f);

        // Restore color
        sr.color = original;

        flashRoutine = null;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
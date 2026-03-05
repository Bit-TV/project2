using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    private SpriteRenderer sr;
    private Coroutine flashRoutine;
    private Color baseColor;

    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        // Works if the SpriteRenderer is on a child too
        sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
            baseColor = sr.color;

        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (sr != null)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(DamageFlash());
        }

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator DamageFlash()
    {
        sr.color = Color.white;

        // Realtime so it still finishes even if you pause mid-flash
        yield return new WaitForSecondsRealtime(0.08f);

        sr.color = baseColor;
        flashRoutine = null;
    }

    private void Die()
{
    //Enemy dies → destroyed normally
   //Player dies → Game Over screen
    GameOverManager gom = FindObjectOfType<GameOverManager>();

    if (CompareTag("Player") && gom != null)
    {
        gom.TriggerGameOver();
        gameObject.SetActive(false);
    }
    else
    {
        Destroy(gameObject);
    }
}
}
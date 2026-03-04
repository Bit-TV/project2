using UnityEngine;

// Simple melee attack that damages enemies in a small area in front of the player.
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public KeyCode attackKey = KeyCode.LeftShift;
    public int damage = 1;
    public float attackRange = 1.6f;     // Distance in front of the player
    public float hitRadius = 1.0f;       // Size of hit circle
    public float attackCooldown = 0.25f; // Time between attacks
    public LayerMask enemyLayer;         // Set this to the Enemy layer in Inspector

    [Header("Knockback")]
    public bool useKnockback = true;
    public float knockbackMultiplier = 1f; // Multiplies the enemy's knockbackForce

    private float cooldownTimer = 0f;
    private Vector2 lastDirection = Vector2.right; // Direction player last moved
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Cooldown timer
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        // Track direction based on movement so attacks aim correctly
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
            lastDirection = rb.linearVelocity.normalized;

        // Attack input (Left Shift)
        if (Input.GetKeyDown(attackKey) && cooldownTimer <= 0f)
        {
            Attack();
            cooldownTimer = attackCooldown;
        }
    }

    private void Attack()
    {
        // Attack point is in front of the player
        Vector2 attackCenter = (Vector2)transform.position + lastDirection * attackRange;

        // Find all enemies in the attack area
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCenter, hitRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            // Damage enemies that have Health
            Health h = hit.GetComponent<Health>();
            if (h != null)
                h.TakeDamage(damage);

            // Optional knockback for impact
            if (useKnockback)
            {
                EnemyAI ai = hit.GetComponent<EnemyAI>();
                if (ai != null)
                {
                    Vector2 knockDir = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
                    ai.ApplyKnockback(knockDir * knockbackMultiplier);
                }
            }
        }
    }

    // Shows the attack circle in Scene view when player is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector2 dir = lastDirection == Vector2.zero ? Vector2.right : lastDirection;
        Vector2 attackCenter = (Vector2)transform.position + dir * attackRange;

        Gizmos.DrawWireSphere(attackCenter, hitRadius);
    }
}
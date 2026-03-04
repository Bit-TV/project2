using UnityEngine;

// Simple melee attack that damages enemies in a small area in front of the player.
public class PlayerAttack : MonoBehaviour
{
    public int damage = 1;
    public float attackRange = 1.2f;      // How far in front the hit happens
    public float hitRadius = 0.6f;        // Size of the hit circle
    public float attackCooldown = 0.35f;  // Time between attacks
    public LayerMask enemyLayer;          // Set to Enemy layer in Inspector

    private float cooldownTimer = 0f;
    private Vector2 lastDirection = Vector2.right; // Direction we last moved (for aiming)
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Cooldown countdown
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        // Update lastDirection based on movement, so attack aims where you're moving
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
            lastDirection = rb.linearVelocity.normalized;

        // Left shift attacks
       if (Input.GetKeyDown(KeyCode.LeftShift) && cooldownTimer <= 0f)
        {
            Attack();
            cooldownTimer = attackCooldown;
        }
    }

    private void Attack()
    {
        Vector2 attackCenter = (Vector2)transform.position + lastDirection * attackRange;

        // Find all enemies in the attack area
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCenter, hitRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Health h = hit.GetComponent<Health>();
            if (h != null)
                h.TakeDamage(damage);
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
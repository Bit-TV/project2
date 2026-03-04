using UnityEngine;

// Enemy chases the player, deals damage on contact (with cooldown),
// and can be knocked back when hit by the player.
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Damage To Player")]
    public int damage = 1;
    public float damageInterval = 1f;

    [Header("Knockback")]
    public float knockbackForce = 4f;

    private Rigidbody2D rb;
    private Transform player;
    private float damageTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;      // Top-down game, no gravity
        rb.freezeRotation = true;  // Prevent spinning
    }

    private void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("No object tagged 'Player' found.");
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Move toward the player
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Only damage the player
        if (!collision.gameObject.CompareTag("Player")) return;

        damageTimer += Time.deltaTime;

        if (damageTimer >= damageInterval)
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);

            damageTimer = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reset timer when no longer touching player
        if (collision.gameObject.CompareTag("Player"))
            damageTimer = 0f;
    }

    // Called by PlayerAttack when the enemy is hit.
    public void ApplyKnockback(Vector2 direction)
    {
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}
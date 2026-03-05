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
    private bool touchingPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;      // Top-down game, no gravity
        rb.freezeRotation = true;  // Prevent spinning
    }

    private void Start()
    {
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

        // Handle damage over time while touching
        if (touchingPlayer)
        {
            damageTimer += Time.fixedDeltaTime;

            if (damageTimer >= damageInterval)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                    playerHealth.TakeDamage(damage);

                damageTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        touchingPlayer = true;
        damageTimer = 0f;

        // Instant first hit
        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damage);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        touchingPlayer = false;
        damageTimer = 0f;
    }

    // Called by PlayerAttack when the enemy is hit.
    public void ApplyKnockback(Vector2 direction)
    {
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}
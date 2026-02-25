using UnityEngine;

// Simple enemy that chases the player and deals damage on contact.
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Damage")]
    public int damage = 1;              // Damage dealt per hit
    public float damageInterval = 1f;   // Time between damage ticks (seconds)

    private Rigidbody2D rb;
    private Transform player;

    private float damageTimer = 0f;     // Tracks time between damage ticks

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;       // Top-down game, no gravity
        rb.freezeRotation = true;   // Prevent spinning on collision
    }

    private void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("No object tagged 'Player' found.");
        }
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
            {
                playerHealth.TakeDamage(damage);
            }

            damageTimer = 0f; // Reset cooldown timer
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reset timer when no longer touching player
        if (collision.gameObject.CompareTag("Player"))
        {
            damageTimer = 0f;
        }
    }
}
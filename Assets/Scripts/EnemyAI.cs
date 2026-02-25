using UnityEngine;

// Simple enemy that moves toward the player.
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("No object tagged 'Player' was found.");
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
    }
}
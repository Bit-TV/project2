using UnityEngine;

// Moves the player around the arena using Rigidbody2D.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;     // top-down, no gravity
        rb.freezeRotation = true; // don't spin on collisions
    }

    private void Update()
    {
        // Read input (WASD / arrow keys)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Normalize prevents faster diagonal movement
        moveInput = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        // Apply velocity for smooth physics movement
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
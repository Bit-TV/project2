using UnityEngine;

// Makes the camera follow a target (the player).
public class CameraFollow : MonoBehaviour
{
    public Transform target;        // The player to follow
    public float smoothSpeed = 5f;  // How smooth the camera movement feels
    public Vector3 offset;          // Offset from the target

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position based on player position + offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move toward that position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
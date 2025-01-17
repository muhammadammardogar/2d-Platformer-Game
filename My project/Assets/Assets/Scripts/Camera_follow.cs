using UnityEngine;

public class CameraFollowWithBounds : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target the camera will follow (usually the player).")]
    public Transform target;

    [Header("Camera Settings")]
    [Tooltip("Horizontal offset of the camera from the player.")]
    public float horizontalOffset = 0f;

    [Tooltip("Smoothness of the camera movement.")]
    public float smoothSpeed = 0.125f;

    [Header("Boundary Settings")]
    [Tooltip("The minimum X position of the camera (left boundary).")]
    public float minX;

    [Tooltip("The maximum X position of the camera (right boundary).")]
    public float maxX;

    private float initialY; // The initial Y position of the camera.

    private void Start()
    {
        // Store the initial Y position of the camera.
        initialY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate the target position for the camera.
            Vector3 targetPosition = new Vector3(target.position.x + horizontalOffset, initialY, transform.position.z);

            // Clamp the target position to stay within boundaries.
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);

            // Smoothly interpolate the camera's position to the target position.
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            // Update the camera's position.
            transform.position = smoothedPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the boundaries in the Scene view for visualization.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minX, initialY - 5, 0), new Vector3(minX, initialY + 5, 0)); // Left boundary
        Gizmos.DrawLine(new Vector3(maxX, initialY - 5, 0), new Vector3(maxX, initialY + 5, 0)); // Right boundary
    }
}

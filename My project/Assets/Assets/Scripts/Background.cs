using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform cameraTransform;  // Reference to the camera

    void Update()
    {
        // Set the background position to match the camera's position
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);
    }
}

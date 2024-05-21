using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // The player transform
    public float smoothSpeed = 0.125f;  // Smoothness factor
    public Vector3 offset;  // Offset of the camera from the player
    private float initialZ;  // Initial Z position of the camera

    private void Start()
    {
        initialZ = transform.position.z;

        // Ensure the camera starts at the correct position relative to the target
        if (target != null)
        {
            Vector3 initialPosition = target.position + offset;
            initialPosition.z = initialZ;  // Keep the initial Z position
            transform.position = initialPosition;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = initialZ;  // Maintain the initial Z position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}

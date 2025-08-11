using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera targetCamera; // The camera to look at

    void Update()
    {
        if (targetCamera != null)
        {
            // Calculate the direction from the object to the camera
            Vector3 lookDirection = targetCamera.transform.position - transform.position;

            // Invert the look direction to face the camera
            lookDirection *= -1f;

            // Ensure that the object maintains its up direction (e.g., doesn't tilt)
            Vector3 upDirection = Vector3.up;

            // Rotate the object to face the camera
            transform.rotation = Quaternion.LookRotation(lookDirection, upDirection);
        }
    }
}

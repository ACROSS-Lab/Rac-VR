using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    [SerializeField] Vector3 distanceFromCamera = new Vector3(0, 0, 6);
    [SerializeField] float smoothSpeed = 8.0f;

    [Header("Deadzone")]
    [SerializeField] float angleThreshold = 7.0f;
    [SerializeField] float distanceThreshold = 0.5f;
    [SerializeField] float stopTolerance = 0.05f;

    Transform cameraTransform;
    bool isAdjusting = false;

    void OnEnable()
    {
        cameraTransform = Camera.main.transform;

        Vector3 targetPosition = cameraTransform.position + (cameraTransform.forward * distanceFromCamera.z)
         + (cameraTransform.up * distanceFromCamera.y) + (cameraTransform.right * distanceFromCamera.x);
        
        transform.position = targetPosition;

        float cameraYaw = cameraTransform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, cameraYaw, 0);
    }

    void LateUpdate()
    {
        // if (cameraTransform == null) cameraTransform = Camera.main.transform;

        // Vector3 targetPosition = cameraTransform.position + (cameraTransform.forward * distanceFromCamera.z)
        //  + (cameraTransform.up * distanceFromCamera.y) + (cameraTransform.right * distanceFromCamera.x);
        // transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // float cameraYaw = cameraTransform.eulerAngles.y;
        // float cameraPitch = cameraTransform.eulerAngles.x;
        // Quaternion targetRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);

        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

        // if (cameraTransform == null) cameraTransform = Camera.main.transform;

        // 1. Calculate the IDEAL target position and rotation
        Vector3 idealPosition = cameraTransform.position + (cameraTransform.forward * distanceFromCamera.z)
         + (cameraTransform.up * distanceFromCamera.y) + (cameraTransform.right * distanceFromCamera.x);

        float cameraYaw = cameraTransform.eulerAngles.y;
        float cameraPitch = cameraTransform.eulerAngles.x;
        Quaternion idealRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);

        // 2. Check if we need to wake up and start moving
        if (!isAdjusting)
        {
            float angleDifference = Quaternion.Angle(transform.rotation, idealRotation);
            float distanceDifference = Vector3.Distance(transform.position, idealPosition);

            // If the camera moved or rotated past our deadzone limits, start tracking
            if (angleDifference > angleThreshold || distanceDifference > distanceThreshold)
            {
                isAdjusting = true;
            }
        }

        // 3. Move the canvas if it's currently in an adjusting state
        if (isAdjusting)
        {
            transform.position = Vector3.Lerp(transform.position, idealPosition, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, idealRotation, Time.deltaTime * smoothSpeed);

            // 4. Check if we've caught up enough to go back to sleep
            float currentAngleDiff = Quaternion.Angle(transform.rotation, idealRotation);
            float currentDistDiff = Vector3.Distance(transform.position, idealPosition);

            if (currentAngleDiff < stopTolerance && currentDistDiff < stopTolerance)
            {
                isAdjusting = false;
            }
        }
    }
}
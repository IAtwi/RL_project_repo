using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    public Transform ball; // Assign the ball object
    public Transform field; // Assign the center of the field (ellipse center)

    [Header("Ellipse Properties")]
    public float ellipseWidth = 36f; // Width of the ellipse ellipseWidth
    public float ellipseLength = 60f; // Width of the ellipse
    public float cameraHeight = 35f; // Adjustable camera height

    [Header("Update Interval")]
    public float updateInterval = 2.5f; // Time in seconds between camera repositioning
    public float moveDuration = 1f; // Time in seconds for the camera to move smoothly

    private void Start()
    {
        // Call UpdateCameraPosition every 'updateInterval' seconds
        InvokeRepeating(nameof(StartCameraMovement), 0f, updateInterval);
    }

    private void StartCameraMovement()
    {
        if (ball == null || field == null) return;

        // Compute the farthest intersection with the ellipse
        Vector3 farIntersection = GetFarEllipseIntersection(ball.position, field.position, ellipseLength, ellipseWidth);

        // Set target position with adjusted camera height
        Vector3 targetPosition = new Vector3(farIntersection.x, cameraHeight, farIntersection.z);

        // Start smooth transition
        StartCoroutine(MoveCameraSmoothly(targetPosition));
    }

    private IEnumerator MoveCameraSmoothly(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            transform.LookAt(field.position);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure the camera reaches exactly the target position
        transform.position = targetPosition;
        transform.LookAt(field.position);
    }

    private Vector3 GetFarEllipseIntersection(Vector3 ballPos, Vector3 center, float a, float b)
    {
        // Translate coordinates relative to ellipse center
        float dx = ballPos.x - center.x;
        float dz = ballPos.z - center.z;

        // Normalize the direction
        float magnitude = Mathf.Sqrt(dx * dx + dz * dz);
        float dirX = dx / magnitude;
        float dirZ = dz / magnitude;

        // Compute intersections (+- solutions using ellipse equation)
        float factor = Mathf.Sqrt(1 / ((dirX * dirX) / (a * a) + (dirZ * dirZ) / (b * b)));

        // Two intersection points
        float nearX = center.x + dirX * factor;
        float nearZ = center.z + dirZ * factor;

        float farX = center.x - dirX * factor;
        float farZ = center.z - dirZ * factor;

        // Select the farther one from the ball
        float distNear = (nearX - ballPos.x) * (nearX - ballPos.x) + (nearZ - ballPos.z) * (nearZ - ballPos.z);
        float distFar = (farX - ballPos.x) * (farX - ballPos.x) + (farZ - ballPos.z) * (farZ - ballPos.z);

        return distFar > distNear ? new Vector3(farX, 0, farZ) : new Vector3(nearX, 0, nearZ);
    }
}

using UnityEngine;

public class GizmosDrawer : MonoBehaviour
{
    [Header("Reset Agents Section")] // Section Header in the Inspector
    public Vector3 localCenter = Vector3.zero; // Rectangle center in LOCAL space
    public float height = 13.2f; // Height along the X-axis
    public float width = 15.0f;  // Width along the Z-axis
    public Color gizmoColor = Color.green;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // Define rectangle corners RELATIVE to the local center
        Vector3 topLeft = localCenter + new Vector3(-height / 2, 0, -width / 2);
        Vector3 topRight = localCenter + new Vector3(height / 2, 0, -width / 2);
        Vector3 bottomLeft = localCenter + new Vector3(-height / 2, 0, width / 2);
        Vector3 bottomRight = localCenter + new Vector3(height / 2, 0, width / 2);

        // Convert local points to world space
        topLeft = transform.TransformPoint(topLeft);
        topRight = transform.TransformPoint(topRight);
        bottomLeft = transform.TransformPoint(bottomLeft);
        bottomRight = transform.TransformPoint(bottomRight);

        // Draw the rectangle
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}

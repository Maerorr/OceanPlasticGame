using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCracks : MonoBehaviour
{
    public LineRenderer lineRenderer; // Reference to the LineRenderer
    public float moveValue = 0f; // Value between 0 and 1 to define the position on the path

    private Vector3[] points;

    void Start()
    {
        // Get the points from the LineRenderer
        int numPoints = lineRenderer.positionCount;
        points = new Vector3[numPoints];
        lineRenderer.GetPositions(points);
    }

    void Update()
    {
        // Ensure moveValue is between 0 and 1
        moveValue = Mathf.Clamp01(moveValue);

        // Get the position on the path
        Vector3 position = GetPositionOnPath(moveValue);

        // Set the object's position
        transform.position = position;
    }

    Vector3 GetPositionOnPath(float t)
    {
        if (points == null || points.Length == 0)
            return Vector3.zero;

        // Calculate the total length of the path
        float totalLength = 0f;
        float[] segmentLengths = new float[points.Length - 1];

        for (int i = 0; i < points.Length - 1; i++)
        {
            segmentLengths[i] = Vector3.Distance(points[i], points[i + 1]);
            totalLength += segmentLengths[i];
        }

        // Find the target distance along the path
        float targetDistance = t * totalLength;
        float cumulativeLength = 0f;

        // Find the segment where the target distance falls
        for (int i = 0; i < segmentLengths.Length; i++)
        {
            if (cumulativeLength + segmentLengths[i] >= targetDistance)
            {
                // Interpolate within this segment
                float segmentT = (targetDistance - cumulativeLength) / segmentLengths[i];
                return Vector3.Lerp(points[i], points[i + 1], segmentT);
            }
            cumulativeLength += segmentLengths[i];
        }

        return points[points.Length - 1];
    }
}

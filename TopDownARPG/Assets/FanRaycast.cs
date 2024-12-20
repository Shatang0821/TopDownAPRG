using System;
using UnityEngine;
using System.Collections.Generic;

public class FanRaycast : MonoBehaviour
{
    public float radius = 5.0f; // ξ^ΜΌa
    public float angle = 45.0f; // ξ^Μpx
    public int rayCount = 10;   // RayΜ
    public float rollAngle = 30.0f; // ξ^Μ[px

    private void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;
        Vector3 origin = transform.position;

        // ξ^Μe|CgπvZ
        List<Vector3> fanPoints = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -angle / 2 + (angle / rayCount) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            fanPoints.Add(origin + direction * radius);
        }

        // [hσΤΕΜ[pxΜKp
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, transform.forward);
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Vector3 localPoint = fanPoints[i] - origin;
            fanPoints[i] = origin + rollRotation * localPoint;
        }

        // MY`ζ
        Gizmos.color = Color.red;
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Gizmos.DrawLine(origin, fanPoints[i]);
        }
    }
}

using System;
using UnityEngine;
using System.Collections.Generic;

public class FanRaycast : MonoBehaviour
{
    public float radius = 5.0f; // îŒ^‚Ì”¼Œa
    public float angle = 45.0f; // îŒ^‚ÌŠp“x
    public int rayCount = 10;   // Ray‚Ì”
    public float rollAngle = 30.0f; // îŒ^‚Ìƒ[ƒ‹Šp“x

    private void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;
        Vector3 origin = transform.position;

        // îŒ^‚ÌŠeƒ|ƒCƒ“ƒg‚ğŒvZ
        List<Vector3> fanPoints = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -angle / 2 + (angle / rayCount) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            fanPoints.Add(origin + direction * radius);
        }

        // ƒ[ƒ‹ƒh‹óŠÔ‚Å‚Ìƒ[ƒ‹Šp“x‚Ì“K—p
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, transform.forward);
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Vector3 localPoint = fanPoints[i] - origin;
            fanPoints[i] = origin + rollRotation * localPoint;
        }

        // ƒMƒYƒ‚•`‰æ
        Gizmos.color = Color.red;
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Gizmos.DrawLine(origin, fanPoints[i]);
        }
    }
}

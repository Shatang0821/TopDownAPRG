using System;
using UnityEngine;
using System.Collections.Generic;

public class FanRaycast : MonoBehaviour
{
    public float radius = 5.0f; // ��^�̔��a
    public float angle = 45.0f; // ��^�̊p�x
    public int rayCount = 10;   // Ray�̐�
    public float rollAngle = 30.0f; // ��^�̃��[���p�x

    private void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;
        Vector3 origin = transform.position;

        // ��^�̊e�|�C���g���v�Z
        List<Vector3> fanPoints = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -angle / 2 + (angle / rayCount) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            fanPoints.Add(origin + direction * radius);
        }

        // ���[���h��Ԃł̃��[���p�x�̓K�p
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, transform.forward);
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Vector3 localPoint = fanPoints[i] - origin;
            fanPoints[i] = origin + rollRotation * localPoint;
        }

        // �M�Y���`��
        Gizmos.color = Color.red;
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Gizmos.DrawLine(origin, fanPoints[i]);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class FanRaycast : MonoBehaviour
{
    public float radius = 5.0f; // 扇型の半径
    public float angle = 45.0f; // 扇型の角度
    public int rayCount = 10;   // Rayの数
    public float rollAngle = 30.0f; // 扇型のロール角度

    void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 origin = transform.position;

        // 扇型の各ポイントを計算
        List<Vector3> fanPoints = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -angle / 2 + (angle / rayCount) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            fanPoints.Add(origin + direction * radius);
        }

        // ワールド空間でのロール角度の適用
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, transform.forward);
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Vector3 localPoint = fanPoints[i] - origin;
            fanPoints[i] = origin + rollRotation * localPoint;
        }

        // Rayを飛ばして衝突判定
        foreach (var point in fanPoints)
        {
            Vector3 direction = (point - origin).normalized;
            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, radius))
            {
                Debug.Log("Hit: " + hit.collider.name);
                // 衝突処理を追加
            }

            // デバッグ用のRayを描画
            Debug.DrawRay(origin, direction * radius, Color.red);
        }
    }
}

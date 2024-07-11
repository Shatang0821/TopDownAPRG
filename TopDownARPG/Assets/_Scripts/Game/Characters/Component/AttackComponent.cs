using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public LayerMask targetLayerMask;       //レイヤーマスク
    public Transform RayStartPoint;         //レイを飛ばす位置

    private HashSet<IDamaged> _hitEntities = new HashSet<IDamaged>();

    /// <summary>
    /// 扇型レイを使って敵のダメージ処理させる
    /// </summary>
    /// <param name="angle">範囲</param>
    /// <param name="rayCount">レイ数</param>
    /// <param name="rollAngle">ロール角度</param>
    /// <param name="radius">半径</param>
    public bool StableRolledFanRayCast(float angle, int rayCount, float rollAngle,float radius,float damage)
    {
        
        Vector3 forward = RayStartPoint.forward;
        Vector3 origin = RayStartPoint.position;

        // 扇型の各ポイントを計算
        List<Vector3> fanPoints = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -angle / 2 + (angle / rayCount) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            fanPoints.Add(origin + direction * radius);
        }

        // ワールド空間でのロール角度の適用
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, RayStartPoint.forward);
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
            if (Physics.Raycast(ray, out RaycastHit hit, radius,targetLayerMask))
            {
                //攻撃が壁など環境にする抜けないように;
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    continue; // レイをスキップする
                }
                //IDamagedインタフェースを使ってダメージ処理
                IDamaged damagedEntity = hit.collider.GetComponent<IDamaged>();
                if (damagedEntity != null && !_hitEntities.Contains(damagedEntity))
                {
                    _hitEntities.Add(damagedEntity);
                    damagedEntity.TakeDamage(damage);
                    
                }
            }
        }
        
        //当たったターゲットがいればtrueを返す
        if (_hitEntities.Count > 0)
        {
            ResetHits();
            return true;
        }
        
        ResetHits();
        return false;
    }

    public void ResetHits()
    {
        _hitEntities.Clear();
    }
}
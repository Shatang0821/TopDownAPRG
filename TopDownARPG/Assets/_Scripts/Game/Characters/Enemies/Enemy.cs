using System;
using FrameWork.FSM;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract  class Enemy : Entity
{
    [SerializeField]
    protected Transform player;
    [HideInInspector]
    public bool TargetFound;    //ターゲット特定
    [HideInInspector]
    public bool InAttackRange;  //ターゲットが攻撃範囲内
    //警戒範囲
    public float DetectionRange = 5.0f;             //警戒範囲
    public float DetectionFieldOfView = 45.0f;      //警戒視野角(度)
    //攻撃範囲
    public float AttackRange = 5.0f;
    public float AttackFieldOfView = 45.0f; 
    protected StateMachine enemyStateMachine;

    protected override void Awake()
    {
        base.Awake();
        enemyStateMachine = CreateStateMachine();
    }
    
    protected virtual void Start()
    {
        enemyStateMachine.ChangeState(GetInitialState());
    }

    protected virtual void Update()
    {
        enemyStateMachine.LogicUpdate();
        CheckPlayerRange();
    }
    

    protected abstract StateMachine CreateStateMachine();
    protected abstract Enum GetInitialState();
    
    //
    private Vector3 directionToPlayer => (player.position - transform.position).normalized;
    void CheckPlayerRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // 警戒範囲内かつ視野内にいるかをチェック
        if (distance <= DetectionRange)
        {
            float detectionAngle = Vector3.Angle(transform.forward, directionToPlayer);
            if (detectionAngle <= DetectionFieldOfView / 2)
            {
                TargetFound = true;
            }
        }
        else
        {
            TargetFound = false;
        }

        // 攻撃範囲内かつ視野内にいるかをチェック
        if (distance <= AttackRange)
        {
            float attackAngle = Vector3.Angle(transform.forward, directionToPlayer);
            if (attackAngle <= AttackFieldOfView / 2)
            {
                InAttackRange = true;
            }
            else
            {
                InAttackRange = false;
            }
        }
        else
        {
            InAttackRange = false;
        }
    }
    
    /// <summary>
    /// オブジェクト同士の角度
    /// </summary>
    /// <param name="self">自身</param>
    /// <param name="target">ターゲット</param>
    /// <returns>角度</returns>
    private float Angle(Vector3 self,Vector3 target)
    {
        float dotProduct = Vector3.Dot(self, target);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        return angle;
    }

    #region Debug

    void OnGUI()
    {
        // デバッグ情報を画面に表示
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.black;
        
        string message = TargetFound ? "Target Found!" : "Target Not Found";
        GUI.Label(new Rect(10, 10, 300, 50), message, style);
        message = InAttackRange ? "Can Attack" : "Can't Attack";
        GUI.Label(new Rect(10, 30, 300, 50), message, style);
    }
    
    void OnDrawGizmos()
    {
        var position = transform.position;
        var forward = transform.forward;
        // 警戒範囲の円を描画
        Gizmos.color = Color.red;
        DrawWireCircle(position, DetectionRange, 0.1f);
        // 攻撃範囲の円を描画
        Gizmos.color = Color.green;
        DrawWireCircle(position,AttackRange,0.1f);
        
        // 視野角の線を描画
        Vector3 leftBoundary = Quaternion.Euler(0, -DetectionFieldOfView / 2, 0) * forward * DetectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, DetectionFieldOfView / 2, 0) * forward * DetectionRange;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, position + leftBoundary);
        Gizmos.DrawLine(position, position + rightBoundary);

        leftBoundary = Quaternion.Euler(0, -AttackFieldOfView / 2, 0) * forward * AttackRange;
        rightBoundary = Quaternion.Euler(0, AttackFieldOfView / 2, 0) * forward * AttackRange;
        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(position, position + leftBoundary);
        Gizmos.DrawLine(position, position + rightBoundary);
    }
    
    void DrawWireCircle(Vector3 center, float radius, float segmentLength)
    {
        int segmentCount = Mathf.CeilToInt(2 * Mathf.PI * radius / segmentLength);
        float angleStep = 360f / segmentCount;

        Vector3 prevPoint = center + new Vector3(radius, 0, 0);
        for (int i = 1; i <= segmentCount; i++)
        {
            float angle = i * angleStep;
            Vector3 newPoint = center + Quaternion.Euler(0, angle, 0) * new Vector3(radius, 0, 0);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }

    #endregion
   
}
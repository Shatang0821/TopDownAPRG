using FrameWork.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using StateMachine = FrameWork.FSM.StateMachine;

public abstract class Enemy : Entity
{
    private Transform playerTransform;
    public bool TargetFound { get; private set; } // ターゲット特定
    public bool InAttackRange { get; private set; } // ターゲットが攻撃範囲内
    public float DetectionRange = 5.0f; // 警戒範囲
    public float DetectionFieldOfView = 45.0f; // 警戒視野角(度)
    public float AttackRange = 5.0f; // 攻撃範囲
    public float AttackFieldOfView = 45.0f; // 攻撃視野角(度)
    protected StateMachine enemyStateMachine;
    public List<AStar.AstarNode> Path { get; private set; }
    public int CurrentPathIndex = 0; // パスの現在添え字
    private bool _finding = false;

    public bool IsTakenDamaged = false; //ダメージを受けたトリガー

    public bool IsUsePool = true; //オブジェクトプールの利用

    // 死亡アクション
    public event Action<Enemy> OnDeath;

    protected override void Awake()
    {
        base.Awake();
        Path = new List<AStar.AstarNode>();
        enemyStateMachine = CreateStateMachine();
    }

    protected virtual void Start()
    {
        enemyStateMachine.ChangeState(GetInitialState());
    }

    public virtual void LogicUpdate()
    {
        enemyStateMachine.LogicUpdate();
        CheckPlayerRange();
    }

    protected void FixedUpdate()
    {
        enemyStateMachine.PhysicsUpdate();
    }


    #region 抽象関数群

    /// <summary>
    /// 移動抽象関数
    /// </summary>
    /// <param name="dir">移動方向</param>
    public abstract void Move(Vector2 dir);

    /// <summary>
    /// 移動中止抽象関数
    /// </summary>
    public abstract void StopMove();

    /// <summary>
    /// ステートマシンの初期化関数
    /// </summary>
    /// <returns>ステートマシンインスタンス</returns>
    protected abstract StateMachine CreateStateMachine();

    /// <summary>
    /// 初期状態を取得する
    /// </summary>
    /// <returns>状態の列挙型</returns>
    protected abstract Enum GetInitialState();

    /// <summary>
    /// ダメージ受けた時の詳細処理(ひるむ値などの計算)
    /// </summary>
    public abstract void TakenDamageState();

    #endregion

    #region API群

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        IsTakenDamaged = true;

        //チンペン音
        AudioManager.Instance.PlayAttack();
    }

    /// <summary>
    /// 死亡処理の呼び出し
    /// </summary>
    public void RaiseOnDeathEvent()
    {
        OnDeath?.Invoke(this);
    }

    /// <summary>
    /// プレイヤーの変換情報を持たせる
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    /// <summary>
    /// パスの探索
    /// </summary>
    public virtual void FindPath()
    {
        if (!_finding)
        {
            Path.Clear();
            _finding = true;
            CurrentPathIndex = 0;
            List<AStar.AstarNode> newPath =
                StageManager.Instance.FindPath(transform.position, playerTransform.position);

            if (newPath != null)
            {
                Path = new List<AStar.AstarNode>(newPath);
            }

            _finding = false;
        }
    }

    /// <summary>
    /// プレイヤの方向正規化済み
    /// </summary>
    public Vector3 DirectionToPlayer => (playerTransform.position - transform.position).normalized;
    
    /// <summary>
    /// プレイヤーまでの距離
    /// </summary>
    public float  DistanceToPlayer => Vector3.Distance(transform.position, playerTransform.position);
    /// <summary>
    /// 警戒攻撃チェック
    /// </summary>
    private void CheckPlayerRange()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 警戒範囲内かつ視野内にいるかをチェック
        if (distance <= DetectionRange)
        {
            float detectionAngle = Vector3.Angle(transform.forward, DirectionToPlayer);
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
            float attackAngle = Vector3.Angle(transform.forward, DirectionToPlayer);
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
    private float Angle(Vector3 self, Vector3 target)
    {
        float dotProduct = Vector3.Dot(self, target);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        return angle;
    }

    #endregion

    #region Debug

    void OnDrawGizmos()
    {
        var position = transform.position;
        var forward = transform.forward;
        // 警戒範囲の円を描画
        Gizmos.color = Color.red;
        DrawWireCircle(position, DetectionRange, 0.1f);
        // 攻撃範囲の円を描画
        Gizmos.color = Color.green;
        DrawWireCircle(position, AttackRange, 0.1f);

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
        //DrawPath();
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

    private void DrawPath()
    {
        if (Path == null || Path.Count == 0)
            return;

        Gizmos.color = Color.green;
        //Debug.Log("パスの長さ" + Path.Count);
        //Debug.Log("Path found: " + string.Join(" -> ", Path.Select(n => n.Pos.ToString()).ToArray()));
        for (int i = 0; i < Path.Count - 1; i++)
        {
            var current = Path[i];
            var next = Path[i + 1];
            Gizmos.DrawLine(
                StageManager.Instance.GridToWorldPosition(current.Pos),
                StageManager.Instance.GridToWorldPosition(next.Pos)
            );
        }
    }

    #endregion
}
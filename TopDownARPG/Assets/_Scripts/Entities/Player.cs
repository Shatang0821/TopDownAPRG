using System;
using System.Collections;
using FrameWork.EventCenter;
using FrameWork.UI;
using FrameWork.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerEvent
{
    Test,
}
public class Player : Entity
{
    //パワーのゲット
    public int Power => power.Value;
    private Camera _camera; 
    private PlayerStateMachine _stateMachine;
    
    #region Component
    public PlayerInputComponent PlayerInputComponent;
    private MovementComponent _movementComponent;
    public AttackComponent AttackComponent;
    #endregion
    
    public Transform RayStartPoint;

    public ComboConfig ComboConfig;
    //移動
    public Vector2 Axis => PlayerInputComponent.Axis;
    //ダッシュ
    public bool DashInput => PlayerInputComponent.Dash;
    //攻撃
    public bool AttackInput => PlayerInputComponent.Attack;
    //攻撃バッファの継続時間
    private float _attackInputBufferTime = 0.2f;            
    //攻撃入力バッファ
    [HideInInspector]
    private WaitForSeconds _waitAttackInputBufferTime;
    //
    public bool HasAttackInputBuffer { get; private set; }
    //被撃
    public bool Damaged = false;
    
    //現在HP
    public float GetCurrentHealth => currentHealth.Value;
    
    /// <summary>
    /// コンポーネントの初期化
    /// </summary>
    private void InitComponent()
    {
        _camera = Camera.main;

        PlayerInputComponent = new PlayerInputComponent();
        PlayerInputComponent.Init();
        
        _movementComponent = new MovementComponent(Rigidbody,transform);
        
        AttackComponent = GetComponent<AttackComponent>();
    }

    public override void InitValue()
    {
        base.InitValue();
        maxHealth = new Observer<float>(100);
        currentHealth = new Observer<float>(maxHealth.Value);
        //テスト
        power = new Observer<int>(10);
        speed = new Observer<float>(5);
    }

    public override void Initialize()
    {
        base.Initialize();
        
        InitComponent();
        
        _waitAttackInputBufferTime = new WaitForSeconds(_attackInputBufferTime);
        _stateMachine = new PlayerStateMachine(this);
        
        _stateMachine.ChangeState(PlayerStateEnum.Idle);
        
        PlayerInputComponent.CurrentDevice.Register(new Action<InputDevice>(OnDeviceChanged));
        currentHealth.Register(new Action<float>(OnCurrentHealthChanged));
        
        PlayerInputComponent.OnEnable();
    }
    
    protected void OnDeviceChanged(InputDevice device)
    {
        Debug.Log($"Maximum Health Changed to: {device}");
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerInputComponent.CurrentDevice.UnRegister(new Action<InputDevice>(OnDeviceChanged));
        currentHealth.UnRegister(new Action<float>(OnCurrentHealthChanged));
        PlayerInputComponent.OnDisable();
    }

    protected override void OnCurrentHealthChanged(float newCurrentHealth)
    {
        base.OnCurrentHealthChanged(newCurrentHealth);
        EventCenter.TriggerEvent(HPBar_EVENT.Change, newCurrentHealth);
    }

    /// <summary>
    /// ロジック更新
    /// </summary>
    public void LogicUpdate()
    {
        _stateMachine.LogicUpdate();
    }

    /// <summary>
    /// 物理更新
    /// </summary>
    public void PhysicsUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    
    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="amount">ダメージ数</param>
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Damaged = true;
    }

    public void Die()
    {
        Destroy(gameObject);
        UIManager.Instance.RemoveAll();
        UIManager.Instance.ShowUI("UIEnd");
        GameManager.Instance.ChangeState(GameState.GameOver);
    }
    
    /// <summary>
    /// アニメーションイベント
    /// </summary>
    private void AnimationEventCalled()
    {
        _stateMachine.AnimationEventCalled();
    }

    private void AnimationEndCalled()
    {
        _stateMachine.AnimationEndCalled();
    }
    
    public void Move()
    {
        if (Axis != Vector2.zero)
        {
            _movementComponent.Move(Axis,speed.Value,0.2f);
        }
    }
    
   /// <summary>
   /// 強制移動
   /// </summary>
   /// <param name="movementDirection"></param>
   /// <param name="speed"></param>
   /// <param name="rotationSpeed"></param>
   /// <param name="rotation"></param>
    public void Move(Vector3 movementDirection,float speed,float rotationSpeed,bool rotation = true)
    {
        _movementComponent.Move(movementDirection,speed,rotationSpeed,rotation);
    }
    
    /// <summary>
    /// ターゲット方向に回転
    /// </summary>
    /// <param name="targetDirection">ターゲット方向</param>
    /// <param name="rotationSpeed">回転速度</param>
    public void RotationWithMouse(Vector3 targetDirection, float rotationSpeed)
    {
        // マウスの位置をスクリーンからワールド座標に変換
        Vector3 mousePosition = PlayerInputComponent.MousePosition;
        mousePosition.z = _camera.transform.position.y; // カメラからの距離を調整
        Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
        
        // プレイヤーの位置からマウスの位置へのベクトルを計算
        Vector3 direction = worldPosition - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            _movementComponent.RotateTowards(transform,direction,rotationSpeed);
        }
    }
    
   
    /// <summary>
    /// パッド入力による回転
    /// </summary>
    /// <param name="inputDirection">入力方向ベクトル</param>
    /// <param name="rotationSpeed">回転速度</param>
    public void RotateWithPad(Vector3 inputDirection, float rotationSpeed)
    {
        // パッド入力がある場合に回転
        if (inputDirection.magnitude > 0.1f)
        {
            _movementComponent.RotateTowards(transform, inputDirection, rotationSpeed);
        }
    } 
    
    
    /// <summary>
    /// コンボのカウントを設定します。
    /// </summary>
    public void SetAttackComboCount()
    {
        animator.SetInteger("ComboCounter", ComboConfig.ComboCount);
        ComboConfig.ComboCount++;
    
        if (ComboConfig.ComboCount > ComboConfig.AttackConfigs.Count) // コンボの最大数を超えたらリセットするなどの処理を追加します
        {
            // コンボのリセット処理など
            ComboConfig.ComboCount = 0;
        }
    }
    
    /// <summary>
    /// ジャンプ入力バッファの設定
    /// </summary>
    public void SetAttackInputBufferTimer()
    {
        StopCoroutine(nameof(AttackInputBufferCoroutine));
        StartCoroutine(nameof(AttackInputBufferCoroutine));
    }

    IEnumerator AttackInputBufferCoroutine()
    {
        HasAttackInputBuffer = true;
        yield return _waitAttackInputBufferTime;
        HasAttackInputBuffer = false;
    }
}
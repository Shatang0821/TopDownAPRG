using System;
using FrameWork.EventCenter;
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
    private Camera _camera; 
    private PlayerStateMachine _stateMachine;
    #region Component
    public PlayerInput _playerInput;
    private MovementComponent _movementComponent;
    public AttackComponent AttackComponent;
    #endregion
    public Transform RayStartPoint;

    public ComboConfig ComboConfig;
    //移動
    public Vector2 Axis => _playerInput.Axis;
    //ダッシュ
    public bool Dash => _playerInput.Dash;
    //攻撃
    public bool Attack => _playerInput.Attack;
    //被撃
    public bool Damaged = false;
    
    private void InitComponent()
    {
        _camera = Camera.main;
        _playerInput = new PlayerInput();
        _movementComponent = new MovementComponent(Rigidbody,transform);
        
        AttackComponent = GetComponent<AttackComponent>();
    }

    protected override void Awake()
    {
        base.Awake();
        InitComponent();
        _stateMachine = new PlayerStateMachine(this);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _stateMachine.ChangeState(PlayerStateEnum.Idle);
        _playerInput.CurrentDevice.Register(new Action<InputDevice>(OnDeviceChanged));
        _playerInput.OnEnable();
    }
    
    protected virtual void OnDeviceChanged(InputDevice device)
    {
        Debug.Log($"Maximum Health Changed to: {device}");
    }
    
    private void Start()
    {
        //maxHealth.Value -= 10.0f;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerInput.CurrentDevice.UnRegister(new Action<InputDevice>(OnDeviceChanged));
        _playerInput.OnDisable();
        
    }

    private void Update()
    {
        _stateMachine.LogicUpdate();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //TakeDamage(5);
        }

        //AttackComponent.StableRolledFanRayCast(_attackConfig.Angle, _attackConfig.RayCount,_attackConfig.RollAngle,_attackConfig.Radius);
        
        StageManager.Instance.WorldToGridPosition(this.transform.position);

//        Rotation();
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Damaged = true;
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
    public void Rotation(Vector3 targetDirection, float rotationSpeed)
    {
        // マウスの位置をスクリーンからワールド座標に変換
        Vector3 mousePosition = _playerInput.MousePosition;
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
}
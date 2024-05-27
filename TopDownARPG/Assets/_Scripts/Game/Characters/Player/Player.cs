using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerEvent
{
    Test,
}
public class Player : Entity
{
    private PlayerStateMachine _stateMachine;
    #region Component
    private PlayerInput _playerInput;
    private MoveComponent _moveComponent;
    #endregion
    public Transform RayStartPoint;

    public ComboConfig ComboConfig;
    //移動
    public Vector2 Axis => _playerInput.Axis;
    //ダッシュ
    public bool Dash => _playerInput.Dash;

    public bool Attack => _playerInput.Attack;

    public bool Damaged = false;

    private float _speed;
    private void InitComponent()
    {
        _playerInput = new PlayerInput();
        _moveComponent = new MoveComponent(Rigidbody,transform);
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
        _playerInput.OnEnable();
    }
    
    private void Start()
    {
        maxHealth.Value -= 10.0f;

        EventCenter.TriggerEvent(PlayerEvent.Test);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerInput.OnDisable();
        
    }

    private void Update()
    {
        _stateMachine.LogicUpdate();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Damage(5);
        }
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    
    public override void Damage(float amount)
    {
        base.Damage(amount);
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
            _moveComponent.Move(Axis,speed.Value);
        }
    }
    
    /// <summary>
    /// スキルによる移動
    /// </summary>
    /// <param name="movementDirection"></param>
    /// <param name="skillSpeed"></param>
    public void Move(Vector3 movementDirection,float skillSpeed)
    {
        {
            _moveComponent.Move(movementDirection,skillSpeed);
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
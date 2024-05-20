using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;

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

    private float _speed;
    private void InitComponent()
    {
        _playerInput = new PlayerInput();
        _moveComponent = new MoveComponent(transform);
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

        EventCenter.AddListener(PlayerEvent.Test,Test);
        EventCenter.AddListener<int>(PlayerEvent.Test,Test1);
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

        EventCenter.RemoveListener(PlayerEvent.Test,Test);
        EventCenter.RemoveListener<int>(PlayerEvent.Test,Test1);
    }
    
    private void Test()
    {
        DebugLogger.Log("Test HPは" + maxHealth.Value);
    }
    
    private void Test1(int i)
    {
        DebugLogger.Log("Test HPは" + i);
    }

    private void Update()
    {
        _stateMachine.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
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
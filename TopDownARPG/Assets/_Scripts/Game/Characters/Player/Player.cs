using FrameWork.Utils;

public class Player : Entity
{
    private PlayerStateMachine _stateMachine;

    #region Component
    public PlayerController PlayerController;
    #endregion

    private void InitComponent()
    {
        PlayerController = new PlayerController(this.transform);
    }

    protected override void Awake()
    {
        base.Awake();
        InitComponent();
        _stateMachine = new PlayerStateMachine(this);
        _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerController.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerController.OnDisable();
    }

    private void Update()
    {
        _stateMachine.LogicUpdate();
        ;
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }

    private void AnimationEventCalled()
    {
        _stateMachine.AnimationEventCalled();
    }
    
    
}
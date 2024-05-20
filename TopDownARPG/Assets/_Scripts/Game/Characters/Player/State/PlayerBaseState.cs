using FrameWork.FSM;
using FrameWork.Utils;


public class PlayerBaseState : BaseState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;

    public PlayerBaseState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName)
    {
        this.player = player;
        this.playerStateMachine = stateMachine;
    }

    /// <summary>
    /// 状態に入る処理
    /// </summary>
    public override void Enter()
    {
        DebugLogger.Log(this.GetType().ToString() + "Enter");
        player.SetAnimation(StateBoolHash, true);
    }

    /// <summary>
    /// 状態から退出処理
    /// </summary>
    public override void Exit()
    {
        player.SetAnimation(StateBoolHash, false);
    }

    public override void HandleInput()
    {
    }

    public override void LogicUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
    }

    public override void AnimationEventCalled()
    {
    }

    public override void AnimationEndCalled()
    {
    }
}
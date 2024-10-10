using FrameWork.FSM;
using FrameWork.Utils;
using UnityEngine;


public class PlayerBaseState : BaseState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;
    protected PlayerInputComponent playerInputComponent;
    public PlayerBaseState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName)
    {
        this.player = player;
        this.playerStateMachine = stateMachine;
        playerInputComponent = player.GetComponent<PlayerInputComponent>();
        if(playerInputComponent == null) Debug.LogError("PlayerInputComponentが見つかりません");
    }

    /// <summary>
    /// 状態に入る処理
    /// </summary>
    public override void Enter()
    {
        stateTimer = 0;
        //DebugLogger.Log(this.GetType().ToString() + "Enter");
        player.SetAnimation(StateBoolHash, true);
    }

    /// <summary>
    /// 状態から退出処理
    /// </summary>
    public override void Exit()
    {
        player.SetAnimation(StateBoolHash, false);
    }
    

    public override void LogicUpdate()
    {
        stateTimer += Time.deltaTime;
        
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
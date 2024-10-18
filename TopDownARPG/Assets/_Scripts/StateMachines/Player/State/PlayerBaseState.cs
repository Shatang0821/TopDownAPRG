using FrameWork.FSM;
using FrameWork.Utils;
using UnityEngine;


public class PlayerBaseState : BaseState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;
    protected PlayerInputComponent playerInputComponent;
    
    protected Animator animator;
    protected PlayerStateConfig playerStateConfig;

    protected string stateConfigPath = "Config & Data/StateConfig/PlayerStateConfig/";

    protected static float transitionDuration;
    public PlayerBaseState(string animName, Player player, PlayerStateMachine stateMachine) : base(animName)
    {
        this.player = player;
        this.playerStateMachine = stateMachine;
        transitionDuration = 0.0f;
        
        playerInputComponent = player.GetComponent<PlayerInputComponent>();
        if(playerInputComponent == null) Debug.LogError("PlayerInputComponentが見つかりません");
        animator = player.GetComponent<Animator>();
        if(animator == null) Debug.LogError("Animatorが見つかりません");
    }

    /// <summary>
    /// 状態に入る処理
    /// </summary>
    public override void Enter()
    {
        stateTimer = 0;
        animator.CrossFade(StateBoolHash, transitionDuration);
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

    /// <summary>
    /// ステート変更処理
    /// </summary>
    /// <param name="state"></param>
    protected void ChangeState(PlayerStateEnum state)
    {
        //遷移時間の設定
        transitionDuration = playerStateConfig.GetTransitionDuration(state.ToString());
        Debug.Log(state.ToString() + "へ遷移,時間は:" + transitionDuration);
        playerStateMachine.ChangeState(state);
    }
}
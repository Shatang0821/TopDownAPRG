using FrameWork.FSM;
using FrameWork.Utils;
using UnityEngine;


public class PlayerBaseState : BaseState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;
    protected PlayerInputComponent playerInputComponent;
    protected CooldownManager _cooldownManager;
    
    protected Animator animator;
    protected PlayerStateConfig playerStateConfig;

    protected string stateConfigPath = "Config & Data/StateConfig/Player/";

    protected static float transitionDuration;
    public PlayerBaseState(string animName, Player player, PlayerStateMachine stateMachine) : base(animName)
    {
        this.player = player;
        this.playerStateMachine = stateMachine;
        transitionDuration = 0.0f;
        
        playerInputComponent = player.GetComponent<PlayerInputComponent>();
        if(playerInputComponent == null) Debug.LogError("PlayerInputComponentが見つかりません");
        _cooldownManager = player.GetComponent<CooldownManager>();
        if(!_cooldownManager) Debug.LogError("CooldownManagerが見つかりません"); 
        animator = player.GetComponent<Animator>();
        if(animator == null) Debug.LogError("Animatorが見つかりません");
    }

    /// <summary>
    /// 状態に入る処理
    /// </summary>
    public override void Enter()
    {
        stateTimer = 0;
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == StateHash)
        {
            // 同じアニメーションを再生
            animator.CrossFade(StateHash, transitionDuration, 0, 0.0f);
        }
        else
        {
            // アニメーションが再生中でなければ、通常通りCrossFadeを使用して再生
            animator.CrossFade(StateHash, transitionDuration);
        }
        //animator.CrossFade(StateHash, transitionDuration);
    }

    /// <summary>
    /// 状態から退出処理
    /// </summary>
    public override void Exit()
    {
        
    }
    

    public override void LogicUpdate()
    {
        stateTimer += Time.deltaTime;
        
    }

    public override void PhysicsUpdate()
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
        if (state == PlayerStateEnum.Attack && _cooldownManager.IsOnCooldown("Attack")) return;
        if (state == PlayerStateEnum.Dash && _cooldownManager.IsOnCooldown("Dash")) return;
        playerStateMachine.ChangeState(state);
    }
}
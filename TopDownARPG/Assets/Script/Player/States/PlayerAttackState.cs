using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private bool _animationDone = false;
    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        player.SetAnimation(Animator.StringToHash("Attack"),true);
        _animationDone = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetAnimation(Animator.StringToHash("Attack"),false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // アニメーションが終了しているか確認
        if (!_animationDone && player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            _animationDone = true;
        }
        // アニメーションが終わっていたら状態遷移をチェック
        if (_animationDone)
        {
            if(player.PlayerController.Axis == Vector2.zero)
                playerStateMachine.ChangeState(PlayerStateEnum.Idle);
            if(player.PlayerController.Axis != Vector2.zero)
                playerStateMachine.ChangeState(PlayerStateEnum.Run);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
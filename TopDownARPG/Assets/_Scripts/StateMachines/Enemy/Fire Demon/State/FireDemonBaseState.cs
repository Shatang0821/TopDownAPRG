using UnityEngine;

public class FireDemonBaseState : EnemyBaseState
{
    protected static float transitionDuration;
    
    protected FireDemon fireDemon;
    public FireDemonBaseState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        fireDemon = enemy as FireDemon;
        if(!fireDemon) {Debug.Log("EnemyをFireDemonに変換失敗");}
    }

    public override void Enter()
    {
        base.Enter();
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
    }

    /// <summary>
    /// ステート変更処理
    /// </summary>
    /// <param name="state"></param>
    protected void ChangeState(FDStateEnum state)
    {
        //遷移時間の設定
        transitionDuration = enemyStateConfig.GetTransitionDuration(state.ToString());

        enemyStateMachine.ChangeState(state);
    }
}
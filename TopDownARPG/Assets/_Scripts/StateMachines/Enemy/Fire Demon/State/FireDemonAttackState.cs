using FrameWork.Audio;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class FireDemonAttackState : FireDemonBaseState
{
    private float angle = 45;        //範囲角度
    private int rayCount = 5;       //レイ数
    private float rollAngle = 0;    //回転角度
    private float radius = 1.3f;       //半径

    private float _attackTiming = 0.55f;
    private bool _isAttacked = false;

    private AttackComponent _attackComponent;

    public FireDemonAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        enemyStateConfig = ResManager.Instance.GetAssetCache<FireDemonStateConfig>(stateConfigPath + "FireDemon/FireDemonAttack_Config");
        _attackComponent = fireDemon.GetComponent<AttackComponent>();
        if(!_attackComponent)Debug.LogWarning("Attack Component is null");
    }

    public override void Enter()
    {
        base.Enter();
        
        _isAttacked = false;

        //チンペン音
        AudioManager.Instance.PlayCRE_Attack();
        DebugLogger.Log("In Attack");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        fireDemon.Rotation(enemy.DirectionToPlayer,0.1f);
        
        if (!_isAttacked && stateTimer > _attackTiming)
        {
            _isAttacked = true;
            _attackComponent.StableRolledFanRayCast(angle, rayCount, rollAngle, radius, 10);
        }
        
        if (enemy.IsTakenDamaged)
        {
            enemy.TakenDamageState();
            return;
        }
        
        
        if (stateTimer > 1.2f)
        {
            if (enemy.TargetFound)
            {

                enemyStateMachine.ChangeState(FDStateEnum.Move);
                return;
            }
            else
            {
                enemyStateMachine.ChangeState(FDStateEnum.Idle);
                return;
            }
        }

        
        
    }

    public override void Exit()
    {
        base.Exit();
        DebugLogger.Log("Out Attack");
    }
}
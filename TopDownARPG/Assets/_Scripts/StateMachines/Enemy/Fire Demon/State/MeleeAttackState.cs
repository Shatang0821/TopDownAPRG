using FrameWork.Utils;
using UnityEngine;

public class MeleeAttackState : EnemyBaseState
{
    private float angle = 45;        //範囲角度
    private int rayCount = 5;       //レイ数
    private float rollAngle = 0;    //回転角度
    private float radius = 1.3f;       //半径

    private float _attackTiming = 0.55f;
    private bool _isAttacked = false;
    private FireDemon _fireDemon;
    public MeleeAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        _fireDemon = enemy as FireDemon;
        if(!_fireDemon) {DebugLogger.Log("EnemyをMeleeに変換失敗");}
    }

    public override void Enter()
    {
        base.Enter();
        
        _isAttacked = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _fireDemon.Rotation(enemy.DirectionToPlayer,0.1f);
        
        if (!_isAttacked && stateTimer > _attackTiming)
        {
            _isAttacked = true;
            if (_fireDemon != null) _fireDemon.AttackComponent.StableRolledFanRayCast(angle, rayCount, rollAngle, radius, 10);
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
}
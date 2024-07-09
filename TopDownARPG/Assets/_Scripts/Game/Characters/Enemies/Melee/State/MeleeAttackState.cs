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
    private Melee _melee;
    public MeleeAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        _melee = enemy as Melee;
        if(!_melee) {DebugLogger.Log("EnemyをMeleeに変換失敗");}
    }

    public override void Enter()
    {
        base.Enter();
        
        _isAttacked = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _melee.Rotation(enemy._directionToPlayer,0.1f);
        if (!_isAttacked && stateTimer > _attackTiming)
        {
            _isAttacked = true;
            Debug.Log("Attack");
            if (_melee != null) _melee.AttackComponent.StableRolledFanRayCast(angle, rayCount, rollAngle, radius, 10);
        }
        
        if (enemy.IsTakenDamaged)
        {
            enemy.TakenDamageState();
            return;
        }
        
        
        if (stateTimer > 1.2f)
        {
            enemyStateMachine.ChangeState(MeleeStateEnum.Idle);
            return;
        }

        
        
    }
}
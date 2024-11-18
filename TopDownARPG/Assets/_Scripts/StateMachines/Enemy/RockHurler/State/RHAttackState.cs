using System.Collections;
using System.Collections.Generic;
using FrameWork.Audio;
using FrameWork.Pool;
using FrameWork.Resource;
using Unity.Properties;
using UnityEngine;

public class RHAttackState : RHMovementState
{
    private GameObject RockPrefab; // 岩のプレハブ
    Vector3 _player;
    public RHAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        enemyStateConfig = ResManager.Instance.GetAssetCache<RockHurlerStateConfig>(stateConfigPath + "RockHurler/RockHurlerAttack_Config");
    }

    public override void Enter()
    {
        base.Enter();
        //チンペン音
        AudioManager.Instance.PlayLRE_Attack();
        PoolManager.Release(RockPrefab, rockHurler.BulletLauncher.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.IsTakenDamaged)
        {
            ChangeState(RHStateEnum.Damaged);
            return;
        }

        _player = enemy.DirectionToPlayer;

        enemy.transform.forward = _player;
        
        if (stateTimer >= 1.0)
        {
            enemyStateMachine.ChangeState(RHStateEnum.Idle);
        }
    }
    
}

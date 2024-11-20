using System.Collections;
using System.Collections.Generic;
using FrameWork.Audio;
using FrameWork.Pool;
using FrameWork.Resource;
using Unity.Properties;
using UnityEngine;

public class RHAttackState : RHBaseState
{
    private GameObject RockPrefab; // 岩のプレハブ
    Vector3 _player;
    private float _attackTime = 0.5f;
    private bool _isAttacked;
    public RHAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        enemyStateConfig = ResManager.Instance.GetAssetCache<RockHurlerStateConfig>(stateConfigPath + "RockHurler/RockHurlerAttack_Config");
        RockPrefab = ResManager.Instance.GetAssetCache<GameObject>("Prefabs/Enemies/Rock-Purple");
    }

    public override void Enter()
    {
        base.Enter();
        //チンペン音
        _isAttacked = false;
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (enemy.IsTakenDamaged)
        {
            ChangeState(RHStateEnum.Damaged);
            return;
        }
        
        if (stateTimer > _attackTime && !_isAttacked)
        {
            AudioManager.Instance.PlayLRE_Attack();
            PoolManager.Release(RockPrefab, rockHurler.BulletLauncher.position);
            _isAttacked = true;
            return;
        }
        
        
        
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

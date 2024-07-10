using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class RHAttackState : RHMovementState
{
    private GameObject RockPrefab; // 岩のプレハブ
    Vector3 _player;
    public RHAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _player = enemy.DirectionToPlayer;

        enemy.transform.forward = _player;

        if (stateTimer >= 1.0)
        {
            enemyStateMachine.ChangeState(RHStateEnum.Idle);
        }
    }

    public override void AnimationEventCalled()
    {
        base.AnimationEventCalled();
        // 岩のプレハブをロード（Resources フォルダー内に配置されていると仮定）
        RockPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Rock-Purple");
        // プレハブを生成して初期化する
        GameObject projectile = GameObject.Instantiate(RockPrefab, enemy.transform.position, Quaternion.identity);
    }
}

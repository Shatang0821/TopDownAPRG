using System.Collections;
using System.Collections.Generic;
using FrameWork.Pool;
using Unity.Properties;
using UnityEngine;

public class RHAttackState : RHMovementState
{
    private GameObject RockPrefab; // 岩のプレハブ
    private RockHurler _rockHurler;
    Vector3 _player;
    public RHAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        _rockHurler = enemy as RockHurler;
        if (!_rockHurler)
        {
            Debug.LogWarning("キャッシュできませんでした");
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.IsTakenDamaged)
        {
            enemy.TakenDamageState();
            return;
        }

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
        GameObject projectile =
            //PoolManager.Release(RockPrefab, _rockHurler.BulletLauncher.position, Quaternion.identity);
            GameObject.Instantiate(RockPrefab, _rockHurler.BulletLauncher.position, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using FrameWork.Pool;
using Unity.Properties;
using UnityEngine;

public class RHAttackState : RHMovementState
{
    private GameObject RockPrefab; // ��̃v���n�u
    private RockHurler _rockHurler;
    Vector3 _player;
    public RHAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        _rockHurler = enemy as RockHurler;
        if (!_rockHurler)
        {
            Debug.LogWarning("�L���b�V���ł��܂���ł���");
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
        // ��̃v���n�u�����[�h�iResources �t�H���_�[���ɔz�u����Ă���Ɖ���j
        RockPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Rock-Purple");
        // �v���n�u�𐶐����ď���������
        GameObject projectile =
            //PoolManager.Release(RockPrefab, _rockHurler.BulletLauncher.position, Quaternion.identity);
            GameObject.Instantiate(RockPrefab, _rockHurler.BulletLauncher.position, Quaternion.identity);
    }
}

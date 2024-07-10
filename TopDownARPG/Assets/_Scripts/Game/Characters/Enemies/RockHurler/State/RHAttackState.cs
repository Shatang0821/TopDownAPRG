using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class RHAttackState : RHMovementState
{
    private GameObject RockPrefab; // ��̃v���n�u
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
        // ��̃v���n�u�����[�h�iResources �t�H���_�[���ɔz�u����Ă���Ɖ���j
        RockPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Rock-Purple");
        // �v���n�u�𐶐����ď���������
        GameObject projectile = GameObject.Instantiate(RockPrefab, enemy.transform.position, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using FrameWork.Resource;
using UnityEngine;

public class RHMoveState : RHMovementState
{
    Vector3 _player;
    float _time;
    float _speed;
    public RHMoveState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        enemyStateConfig = ResManager.Instance.GetAssetCache<RockHurlerStateConfig>(stateConfigPath + "RockHurler/RockHurlerMove_Config");
    }

    public override void Enter()
    {
        base.Enter();
        _time = 0f;
        _speed = 3f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!enemyStateMachine.CheckState(this)) return;


        Movement();

        if (!enemy.TargetFound)
        {
            ChangeState(RHStateEnum.Idle);
        }

        _time += Time.deltaTime;
    }

    #region �G�̈ړ�
    void Movement()
    {
        _player = -enemy.DirectionToPlayer;

        _player.y = 0f;
        // �֍s�^���̂��߂Ƀ����_���ȗv�f��������
        float noise = Mathf.PerlinNoise(_time, 0f) * 3f - 1f;
        Vector3 perpDirection = Vector3.Cross(_player, Vector3.up).normalized;
        _player += perpDirection * noise;


        
        enemy.transform.position += _player * (_speed * Time.deltaTime);

        enemy.transform.forward = _player;
    }
    #endregion
}

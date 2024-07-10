using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHMoveState : RHMovementState
{
    Vector3 _player;
    float _time;
    float _speed;
    public RHMoveState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
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

        Movement();

        if (!enemy.TargetFound)
        {
            enemyStateMachine.ChangeState(RHStateEnum.Idle);
        }

        _time += Time.deltaTime;
    }

    #region “G‚ÌˆÚ“®
    void Movement()
    {
        _player = -enemy.DirectionToPlayer;

        _player.y = 0f;
        // Ös‰^“®‚Ì‚½‚ß‚Éƒ‰ƒ“ƒ_ƒ€‚È—v‘f‚ğ‰Á‚¦‚é
        float noise = Mathf.PerlinNoise(_time, 0f) * 3f - 1f;
        Vector3 perpDirection = Vector3.Cross(_player, Vector3.up).normalized;
        _player += perpDirection * noise;


        
        enemy.transform.position += _player * _speed * Time.deltaTime;

        enemy.transform.forward = _player;
    }
    #endregion
}

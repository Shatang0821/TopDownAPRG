using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FrameWork.Audio;


public class PlayerAttackState : PlayerBaseState
{
    private bool _animetionEnd;
    private bool _canOtherState;
    private bool _isAttacked = false;
    private ComboConfig _comboConfig;
    private AttackConfig _attackConfig;
    private HashSet<Enemy> _hitEnemies;
    private Vector3 toMouseDir; //�}�E�X��������
    public PlayerAttackState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
        _hitEnemies = new HashSet<Enemy>();
    }

    public override void Enter()
    {
        player.SetAttackComboCount();
        base.Enter();
        _isAttacked = false;
        _comboConfig = player.ComboConfig;
        _attackConfig = _comboConfig.AttackConfigs[_comboConfig.ComboCount - 1];
        
        //�}�E�X����̏ꍇ�}�E�X�ʒu�ɉ�]
        if (player._playerInput.CurrentDevice.Value == Keyboard.current)
        {
            toMouseDir = new Vector3(player._playerInput.MousePosition.x, 0, player._playerInput.MousePosition.y);
            player.Rotation(toMouseDir,0f);
        }

        //�`���y����
        AudioManager.Instance.PlayAttack_E();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        var comboConfig = player.ComboConfig;
        var attackConfig = comboConfig.AttackConfigs[comboConfig.ComboCount-1];
        
        //player.AttackComponent.StableRolledFanRayCast(_attackConfig.Angle, _attackConfig.RayCount,_attackConfig.RollAngle,_attackConfig.Radius);
    
        if (!_isAttacked && stateTimer > _attackConfig.AttackTiming)
        {
            _isAttacked = true;
            player.AttackComponent.StableRolledFanRayCast(_attackConfig.Angle, _attackConfig.RayCount,_attackConfig.RollAngle,_attackConfig.Radius,player.Power);
            
        }
        
        if (player.Damaged)
        {
            player.ComboConfig.ComboCount = 0;
            playerStateMachine.ChangeState(PlayerStateEnum.Damaged);
            return;
        }
        if (_canOtherState)
        {
            if (player.Attack && comboConfig.ComboCount < comboConfig.AttackConfigs.Count)
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Attack);
                return;
            }
            if (player.Axis != Vector2.zero)
            {
                player.ComboConfig.ComboCount = 0;
                playerStateMachine.ChangeState(PlayerStateEnum.Move);
                return;
            }
        }

        if (_animetionEnd)
        {
            player.ComboConfig.ComboCount = 0;
            if (player.Axis != Vector2.zero)
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Move);
            }
            else
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Idle);
            }
        }
        
        
    }

    public override void AnimationEventCalled()
    {
        base.AnimationEventCalled();
        _canOtherState = true;
    }

    public override void AnimationEndCalled()
    {
        base.AnimationEndCalled();
        _animetionEnd = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateTimer > _attackConfig.StartMoveTime && stateTimer < _attackConfig.StopMoveTime)
        {
            MovePlayer();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _canOtherState = false;
        _animetionEnd = false;
        toMouseDir = Vector3.zero;
        _hitEnemies.Clear();
    }
    
    private void MovePlayer()
    {
        var forward = player.transform.forward;
        player.Move(new Vector3(forward.x,forward.z,0),_attackConfig.Speed,0,false);
    }
    
    

}
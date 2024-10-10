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
    private Vector3 _toMouseDir; //�}�E�X��������
    private Vector3 _toStickDir; //�X�e�B�b�N��������
    private Camera _camera; //�J����
    
    //Component
    private AttackComponent _attackComponent;
    private MovementComponent _movementComponent;
    
    
    public PlayerAttackState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
        _hitEnemies = new HashSet<Enemy>();
        
        _attackComponent = player.GetComponent<AttackComponent>();
        if(_attackComponent == null) Debug.LogError("AttackComponent��������܂���");
        _movementComponent = player.GetComponent<MovementComponent>();
        if(_movementComponent == null) Debug.LogError("MovementComponent��������܂���");
        
        _camera = Camera.main;
    }

    public override void Enter()
    {
        player.SetAttackComboCount();
        base.Enter();
        _isAttacked = false;
        _comboConfig = player.ComboConfig;
        _attackConfig = _comboConfig.AttackConfigs[_comboConfig.ComboCount - 1];
        
        //�}�E�X����̏ꍇ�}�E�X�ʒu�ɉ�]
        if (playerInputComponent.CurrentDevice.Value == Keyboard.current)
        {
           RotationWithMouse(0f);
        }
        else if (playerInputComponent.CurrentDevice.Value == Gamepad.current)
        {
            _toStickDir = new Vector3(playerInputComponent.Axis.x, 0, playerInputComponent.Axis.y);
            RotateWithPad(_toStickDir,0f);
        }

        //�`���y����
        AudioManager.Instance.PlayAttack_E();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        var comboConfig = player.ComboConfig;
        
        if (!_isAttacked && stateTimer > _attackConfig.AttackTiming)
        {
            _isAttacked = true;
            _attackComponent.StableRolledFanRayCast(_attackConfig.Angle, _attackConfig.RayCount,
                _attackConfig.RollAngle, _attackConfig.Radius, player.Power);
            
        }

        if (playerInputComponent.Attack)
        {
            playerInputComponent.SetAttackInputBufferTimer();
        }
        
        if (player.Damaged)
        {
            player.ComboConfig.ComboCount = 0;
            playerStateMachine.ChangeState(PlayerStateEnum.Damaged);
            return;
        }
        if (_canOtherState)
        {
            if ((playerInputComponent.HasAttackInputBuffer|| playerInputComponent.Attack) && comboConfig.ComboCount < comboConfig.AttackConfigs.Count)
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Attack);
                return;
            }
            if (playerInputComponent.Axis != Vector2.zero)
            {
                player.ComboConfig.ComboCount = 0;
                playerStateMachine.ChangeState(PlayerStateEnum.Move);
                return;
            }
        }

        if (_animetionEnd)
        {
            player.ComboConfig.ComboCount = 0;
            if (playerInputComponent.Axis != Vector2.zero)
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
        _toMouseDir = Vector3.zero;
        _toStickDir = Vector3.zero;
        _hitEnemies.Clear();
    }
    
    private void MovePlayer()
    {
        var forward = player.transform.forward;
        _movementComponent.Move(new Vector3(forward.x,forward.z,0),_attackConfig.Speed,0,false);
    }
    
    /// <summary>
    /// �^�[�Q�b�g�����ɉ�]
    /// </summary>
    /// <param name="targetDirection">�^�[�Q�b�g����</param>
    /// <param name="rotationSpeed">��]���x</param>
    public void RotationWithMouse(float rotationSpeed)
    {
        // �}�E�X�̈ʒu���X�N���[�����烏�[���h���W�ɕϊ�
        Vector3 mousePosition = playerInputComponent.MousePosition;
        mousePosition.z = _camera.transform.position.y; // �J��������̋����𒲐�
        Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
        
        // �v���C���[�̈ʒu����}�E�X�̈ʒu�ւ̃x�N�g�����v�Z
        Vector3 direction = worldPosition - player.transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            _movementComponent.RotateTowards(player.transform,direction,rotationSpeed);
        }
    }
    
    /// <summary>
    /// �p�b�h���͂ɂ���]
    /// </summary>
    /// <param name="inputDirection">���͕����x�N�g��</param>
    /// <param name="rotationSpeed">��]���x</param>
    public void RotateWithPad(Vector3 inputDirection, float rotationSpeed)
    {
        // �p�b�h���͂�����ꍇ�ɉ�]
        if (inputDirection.magnitude > 0.1f)
        {
            _movementComponent.RotateTowards(player.transform, inputDirection, rotationSpeed);
        }
    } 

}
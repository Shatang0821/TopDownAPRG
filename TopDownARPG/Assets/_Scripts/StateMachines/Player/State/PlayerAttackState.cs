using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.InputSystem;
using FrameWork.Audio;
using FrameWork.Resource;


public class PlayerAttackState : PlayerBaseState
{
    private HashSet<Enemy> _hitEnemies;
    private Vector3 _toMouseDir; //�}�E�X��������
    private Vector3 _toStickDir; //�X�e�B�b�N��������
    private Camera _camera; //�J����
    private bool _raycastTrigger = false; //���C�L���X�g�g���K�[
    //Component
    private AttackComponent _attackComponent;
    private MovementComponent _movementComponent;
    //Config
    private AttackConfig _attackConfig;
    private ComboConfig _comboConfig;
    
    public PlayerAttackState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
        _hitEnemies = new HashSet<Enemy>();
        _camera = Camera.main;
        
        _attackComponent = player.GetComponent<AttackComponent>();
        if(_attackComponent == null) Debug.LogError("AttackComponent��������܂���");
        _movementComponent = player.GetComponent<MovementComponent>();
        if(_movementComponent == null) Debug.LogError("MovementComponent��������܂���");
        _comboConfig = ResManager.Instance.GetAssetCache<ComboConfig>("Config & Data/ComboConfig/Katana/KatanaCombo");
        if(_comboConfig == null) Debug.LogError("ComboConfig��������܂���");
        playerStateConfig = ResManager.Instance.GetAssetCache<PlayerStateConfig>(stateConfigPath + "PlayerAttack01_Config");
        if(playerStateConfig == null) Debug.LogError("PlayerAttack01_Config��������܂���");
    }

    public override void Enter()
    {
        SetAttackComboCount();
        base.Enter();
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
        
        // ���C�L���X�g�g���K�[���ԂɂȂ����烌�C�L���X�g���s��
        if (!_raycastTrigger && stateTimer > _attackConfig.RaycastTriggerTime)
        {
            _raycastTrigger = true;
            _attackComponent.StableRolledFanRayCast(_attackConfig.Angle, _attackConfig.RayCount,
                _attackConfig.RollAngle, _attackConfig.Radius, player.Power);
        }
        
        // �U�����͂�����΃o�b�t�@��ݒ�
        if (playerInputComponent.Attack)
        {
            playerInputComponent.SetAttackInputBufferTimer();
        }
        // �������b�N���ԓ��ɍU�����͂�����΍U����ԂɑJ��
        if (playerStateConfig.partialLockTime < stateTimer)
        {
            // if ((playerInputComponent.HasAttackInputBuffer|| playerInputComponent.Attack) && _comboConfig.ComboCount < _comboConfig.AttackConfigs.Count)
            // {
            //     playerStateMachine.ChangeState(PlayerStateEnum.Attack);
            //     return;
            // }
            if (playerInputComponent.Axis != Vector2.zero)
            {
                ChangeState(PlayerStateEnum.Move);
                return;
            }
        }
        
        // ���S���b�N���ԏI������
        if (playerStateConfig.fullLockTime < stateTimer)
        {
            if (playerInputComponent.Axis != Vector2.zero)
            {
                ChangeState(PlayerStateEnum.Move);
            }
            else
            {
                ChangeState(PlayerStateEnum.Idle);
            }
        }
        
        
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
        _raycastTrigger = false;
        _comboConfig.ComboCount = 0;
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
    
    /// <summary>
    /// �R���{�̃J�E���g��ݒ肵�܂��B
    /// </summary>
    public void SetAttackComboCount()
    {
        animator.SetInteger("ComboCounter", _comboConfig.ComboCount);
        _comboConfig.ComboCount++;
    
        if (_comboConfig.ComboCount > _comboConfig.AttackConfigs.Count) // �R���{�̍ő吔�𒴂����烊�Z�b�g����Ȃǂ̏�����ǉ����܂�
        {
            // �R���{�̃��Z�b�g�����Ȃ�
            _comboConfig.ComboCount = 0;
        }
    }

}
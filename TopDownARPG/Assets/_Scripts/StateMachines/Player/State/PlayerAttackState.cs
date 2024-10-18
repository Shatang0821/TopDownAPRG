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
    private Vector3 _toMouseDir; //マウス向き方向
    private Vector3 _toStickDir; //スティック向き方向
    private Camera _camera; //カメラ
    private bool _raycastTrigger = false; //レイキャストトリガー
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
        if(_attackComponent == null) Debug.LogError("AttackComponentが見つかりません");
        _movementComponent = player.GetComponent<MovementComponent>();
        if(_movementComponent == null) Debug.LogError("MovementComponentが見つかりません");
        _comboConfig = ResManager.Instance.GetAssetCache<ComboConfig>("Config & Data/ComboConfig/Katana/KatanaCombo");
        if(_comboConfig == null) Debug.LogError("ComboConfigが見つかりません");
        playerStateConfig = ResManager.Instance.GetAssetCache<PlayerStateConfig>(stateConfigPath + "PlayerAttack01_Config");
        if(playerStateConfig == null) Debug.LogError("PlayerAttack01_Configが見つかりません");
    }

    public override void Enter()
    {
        SetAttackComboCount();
        base.Enter();
        _attackConfig = _comboConfig.AttackConfigs[_comboConfig.ComboCount - 1];
        
        //マウス操作の場合マウス位置に回転
        if (playerInputComponent.CurrentDevice.Value == Keyboard.current)
        {
           RotationWithMouse(0f);
        }
        else if (playerInputComponent.CurrentDevice.Value == Gamepad.current)
        {
            _toStickDir = new Vector3(playerInputComponent.Axis.x, 0, playerInputComponent.Axis.y);
            RotateWithPad(_toStickDir,0f);
        }

        //チンペン音
        AudioManager.Instance.PlayAttack_E();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // レイキャストトリガー時間になったらレイキャストを行う
        if (!_raycastTrigger && stateTimer > _attackConfig.RaycastTriggerTime)
        {
            _raycastTrigger = true;
            _attackComponent.StableRolledFanRayCast(_attackConfig.Angle, _attackConfig.RayCount,
                _attackConfig.RollAngle, _attackConfig.Radius, player.Power);
        }
        
        // 攻撃入力があればバッファを設定
        if (playerInputComponent.Attack)
        {
            playerInputComponent.SetAttackInputBufferTimer();
        }
        // 部分ロック時間内に攻撃入力があれば攻撃状態に遷移
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
        
        // 完全ロック時間終えたら
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
    /// ターゲット方向に回転
    /// </summary>
    /// <param name="targetDirection">ターゲット方向</param>
    /// <param name="rotationSpeed">回転速度</param>
    public void RotationWithMouse(float rotationSpeed)
    {
        // マウスの位置をスクリーンからワールド座標に変換
        Vector3 mousePosition = playerInputComponent.MousePosition;
        mousePosition.z = _camera.transform.position.y; // カメラからの距離を調整
        Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
        
        // プレイヤーの位置からマウスの位置へのベクトルを計算
        Vector3 direction = worldPosition - player.transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            _movementComponent.RotateTowards(player.transform,direction,rotationSpeed);
        }
    }
    
    /// <summary>
    /// パッド入力による回転
    /// </summary>
    /// <param name="inputDirection">入力方向ベクトル</param>
    /// <param name="rotationSpeed">回転速度</param>
    public void RotateWithPad(Vector3 inputDirection, float rotationSpeed)
    {
        // パッド入力がある場合に回転
        if (inputDirection.magnitude > 0.1f)
        {
            _movementComponent.RotateTowards(player.transform, inputDirection, rotationSpeed);
        }
    } 
    
    /// <summary>
    /// コンボのカウントを設定します。
    /// </summary>
    public void SetAttackComboCount()
    {
        animator.SetInteger("ComboCounter", _comboConfig.ComboCount);
        _comboConfig.ComboCount++;
    
        if (_comboConfig.ComboCount > _comboConfig.AttackConfigs.Count) // コンボの最大数を超えたらリセットするなどの処理を追加します
        {
            // コンボのリセット処理など
            _comboConfig.ComboCount = 0;
        }
    }

}
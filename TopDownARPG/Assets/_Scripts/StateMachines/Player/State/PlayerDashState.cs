using FrameWork.Audio;
using FrameWork.Resource;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private Vector3 _direction;                     //プレイヤの入力方向
    private MovementComponent _movementComponent;
    private float _speed = 10.0f;
    
    // レイヤー番号（例えば、Layer1とLayer2の衝突を無効にする）
    private int _layer1 = LayerMask.NameToLayer("Player");
    private int _layer2 = LayerMask.NameToLayer("Enemy");
    private int _layer3 = LayerMask.NameToLayer("EnemyBullet");

    public PlayerDashState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName, player, stateMachine)
    {
        _movementComponent = player.GetComponent<MovementComponent>();
        if(_movementComponent == null) Debug.LogError("MovementComponentが見つかりません");
        playerStateConfig = ResManager.Instance.GetAssetCache<PlayerStateConfig>(stateConfigPath + "PlayerDash_Config");
    }

    public override void Enter()
    {
        base.Enter();
        _direction = playerInputComponent.Axis;

        //チンペン音
        AudioManager.Instance.PlayDash();
        
        // 衝突を無効化
        Physics.IgnoreLayerCollision(_layer1, _layer2, true);
        Physics.IgnoreLayerCollision(_layer1,_layer3,true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(stateTimer < playerStateConfig.FullLockTime) return;
        if (playerInputComponent.Axis != Vector2.zero)
        {
            playerStateMachine.ChangeState(PlayerStateEnum.Move);
            return;
        }
        if (playerInputComponent.Axis == Vector2.zero)
        {
            playerStateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateTimer < playerStateConfig.FullLockTime)
        {
            if (_direction != Vector3.zero)
            {
                _movementComponent.Move(_direction,_speed);
            }
            else
            {
                var forward = player.transform.forward;
                _movementComponent.Move(new Vector3(forward.x,forward.z,0),_speed);

            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        // 衝突を無効化
        Physics.IgnoreLayerCollision(_layer1, _layer2, false);
        Physics.IgnoreLayerCollision(_layer1,_layer3,false);
    }
}
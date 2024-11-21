using System;
using System.Collections;
using FrameWork.EventCenter;
using FrameWork.UI;
using FrameWork.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerEvent
{
    Test,
}
public class Player : Entity
{
    private PlayerStateMachine _stateMachine;
    
    public Transform RayStartPoint;

    private PlayerStatusComponent _playerStatusComponent;

    private CooldownManager _cooldownManager;
    //被撃
    public bool Damaged = false;
    
    //現在HP
    public float GetCurrentHealth => currentHealth.Value;

    public float Power => power.Value;
    public float Health => maxHealth.Value;
    public override void InitValue()
    {
        base.InitValue();
        _playerStatusComponent = GetComponent<PlayerStatusComponent>();
        //_cooldownManager.GetComponent<CooldownManager>();
        _playerStatusComponent.Init();
        maxHealth = new Observer<float>(_playerStatusComponent.CurrentStatus.Health);
        currentHealth = new Observer<float>(maxHealth.Value);
        //テスト
        power = new Observer<float>(_playerStatusComponent.CurrentStatus.AttackPower);
        speed = new Observer<float>(_playerStatusComponent.CurrentStatus.Speed);
        
        //_cooldownManager.SetCooldownTime("Dash",_playerStatusComponent.CurrentSpecificStatus.DashCoolTime);
    }

    public override void Initialize()
    {
        base.Initialize();
        
        _stateMachine = new PlayerStateMachine(this);
        
        _stateMachine.ChangeState(PlayerStateEnum.Idle);
        
        currentHealth.Register(new Action<float>(OnCurrentHealthChanged));
    }
    
    

    protected override void OnDisable()
    {
        base.OnDisable();

        currentHealth.UnRegister(new Action<float>(OnCurrentHealthChanged));
    }

    protected override void OnCurrentHealthChanged(float newCurrentHealth)
    {
        base.OnCurrentHealthChanged(newCurrentHealth);
        EventCenter.TriggerEvent(HPBar_EVENT.Change, newCurrentHealth);
    }

    /// <summary>
    /// ロジック更新
    /// </summary>
    public void LogicUpdate()
    {
        _stateMachine.LogicUpdate();
    }

    /// <summary>
    /// 物理更新
    /// </summary>
    public void PhysicsUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    
    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="amount">ダメージ数</param>
    public override void TakeDamage(float amount)
    {
        if(_stateMachine.CurrentStateName == PlayerStateEnum.Dash.ToString())
            return;
        base.TakeDamage(amount);
        Damaged = true;
    }

    public void Die()
    {
        Destroy(gameObject);
        UIManager.Instance.RemoveAll();
        UIManager.Instance.ShowUI("UIEnd");
        GameManager.Instance.ChangeState(GameState.GameOver);
    }
    
}
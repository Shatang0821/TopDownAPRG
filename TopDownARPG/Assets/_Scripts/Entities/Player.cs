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
    //パワーのゲット
    public int Power => power.Value;

    private PlayerStateMachine _stateMachine;
    
    #region Component
    //public PlayerInputComponent PlayerInputComponent;
    #endregion
    
    public Transform RayStartPoint;
    
    //被撃
    public bool Damaged = false;
    
    //現在HP
    public float GetCurrentHealth => currentHealth.Value;
    
    public override void InitValue()
    {
        base.InitValue();
        maxHealth = new Observer<float>(100);
        currentHealth = new Observer<float>(maxHealth.Value);
        //テスト
        power = new Observer<int>(10);
        speed = new Observer<float>(5);
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
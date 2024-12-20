﻿using System;
using FrameWork.EventCenter;
using UnityEngine;

/// <summary>
/// 仮のEntityデータ
/// </summary>
public struct EntityData
{
    private float initialHealth;
    private float initialCurrentHealth;
    private int initialPower;
    private float initialSpeed;
}

public class Entity : MonoBehaviour, IDamageable
{
    protected Observer<float> maxHealth;

    protected Observer<float> currentHealth;

    protected Observer<float> power;

    protected Observer<float> speed;

    protected Animator animator;
    public float GetMaxHealth => maxHealth.Value;           //最大HPを取得する
    public float GetCurrentHealth => currentHealth.Value;   //現在HPを取得する
    public float GetPower => power.Value;                     //攻撃力を取得する
    public float GetSpeed => speed.Value;                   //移動速度を取得する

    [HideInInspector] public Rigidbody Rigidbody;

    public virtual void Initialize()
    {
        InitValue();
        animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
    }
    
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();

        InitValue();
    }

    /// <summary>
    /// データベースから数値を取得
    /// </summary>
    public virtual void InitValue( /*EntityData entityData*/)
    {

    }

    protected virtual void OnEnable()
    {
        //maxHealth.Register(new Action<float>(OnMaxHealthChanged));
        currentHealth.Register(new Action<float>(OnCurrentHealthChanged));
    }

    protected virtual void OnDisable()
    {
        //maxHealth.UnRegister(new Action<float>(OnMaxHealthChanged));
        currentHealth.UnRegister(new Action<float>(OnCurrentHealthChanged));
    }

    protected virtual void OnMaxHealthChanged(float newMaxHealth)
    {
        Debug.Log($"Maximum Health Changed to: {newMaxHealth}");
        // 最大HPの変更に基づいて現在のHPを調整する場合
        currentHealth.Value = Mathf.Min(currentHealth.Value, newMaxHealth);
    }

    protected virtual void OnCurrentHealthChanged(float newCurrentHealth)
    {
        Debug.Log($"Current Health Changed to: {newCurrentHealth}");　
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="amount"></param>
    public virtual void TakeDamage(float amount)
    {
        currentHealth.Value = Mathf.Max(currentHealth.Value - amount, 0);
    }
    
    
}
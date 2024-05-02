﻿
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

public class Entity : MonoBehaviour,IDamaged
{
    protected Observer<float> maxHealth;
    
    protected Observer<float> currentHealth;

    protected Observer<int> power;

    protected Observer<float> speed;

    protected Animator animator;
    protected virtual void Awake()
    {
        InitValue();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// データベースから数値を取得
    /// </summary>
    public void InitValue(/*EntityData entityData*/)
    {
        /*
         *  if(conectDatabase)
         *  maxHealth = new Observer<float>(entityData.initialHealth, "OnMaxHpChange");
         *  currentHealth = new Observer<float>(entityData.initialCurrentHealth, "OnCurrentHpChange");
         *  power = new Observer<int>(entityData.power, "abc");
         *  speed = new Observer<float>(entityData.speed, "cba");
         */
        maxHealth = new Observer<float>(100, "OnMaxHpChange");
        currentHealth = new Observer<float>(100, "OnCurrentHpChange");
        //テスト
        power = new Observer<int>(10, "abc");
        speed = new Observer<float>(10, "cba");
    }

    protected virtual void OnEnable()
    {
        // イベントリスナーを設定
        EventCenter.AddListener<float>("OnMaxHpChange", OnMaxHealthChanged);
        EventCenter.AddListener<float>("OnCurrentHpChange", OnCurrentHealthChanged);
    }

    protected virtual void OnDisable()
    {
        // イベントリスナーを設定
        EventCenter.RemoveListener<float>("OnMaxHpChange", OnMaxHealthChanged);
        EventCenter.RemoveListener<float>("OnCurrentHpChange", OnCurrentHealthChanged);
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

    public virtual void Damage(float amount)
    {
        currentHealth.Value = Mathf.Max(currentHealth.Value - amount, 0);
    }
    
    /// <summary>
    /// アニメーションを変更する
    /// </summary>
    /// <param name="animHash"></param>
    /// <param name="value"></param>
    public virtual void SetAnimation(int animHash, bool value)
    {
        animator.SetBool(animHash,value);
    }
}
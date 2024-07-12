using System;
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

    protected Observer<int> power;

    protected Observer<float> speed;

    protected Animator animator;
    public float GetMaxHealth => maxHealth.Value;           //最大HPを取得する
    public float GetCurrentHealth => currentHealth.Value;   //現在HPを取得する
    public int GetPower => power.Value;                     //攻撃力を取得する
    public float GetSpeed => speed.Value;                   //移動速度を取得する

    [HideInInspector] public Rigidbody Rigidbody;

    public virtual void Initialize()
    {
        animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
    }
    
    protected virtual void Awake()
    {
        InitValue();
        animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// データベースから数値を取得
    /// </summary>
    public void InitValue( /*EntityData entityData*/)
    {
        /*
         *  if(conectDatabase)
         *  maxHealth = new Observer<float>(entityData.initialHealth, "OnMaxHpChange");
         *  currentHealth = new Observer<float>(entityData.initialCurrentHealth, "OnCurrentHpChange");
         *  power = new Observer<int>(entityData.power, "abc");
         *  speed = new Observer<float>(entityData.speed, "cba");
         */
        maxHealth = new Observer<float>(50);
        currentHealth = new Observer<float>(maxHealth.Value);
        //テスト
        power = new Observer<int>(10);
        speed = new Observer<float>(5);
    }

    protected virtual void OnEnable()
    {
        maxHealth.Register(new Action<float>(OnMaxHealthChanged));
        currentHealth.Register(new Action<float>(OnCurrentHealthChanged));
    }

    protected virtual void OnDisable()
    {
        maxHealth.UnRegister(new Action<float>(OnMaxHealthChanged));
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

    /// <summary>
    /// アニメーションを変更する
    /// </summary>
    /// <param name="animHash"></param>
    /// <param name="value"></param>
    public virtual void SetAnimation(int animHash, bool value)
    {
        animator.SetBool(animHash, value);
    }
    
    
}
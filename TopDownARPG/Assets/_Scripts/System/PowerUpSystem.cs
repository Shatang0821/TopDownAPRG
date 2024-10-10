using System;
using FrameWork.Utils;
using UnityEngine;
using UnityEngine.Serialization;

public class PowerUpSystem : Singleton<PowerUpSystem>
{
    private PowerUp<float> _magicPointPowerUp;        // MP強化データ
    private PowerUp<float> _healthPowerUp;            // HP強化データ
    private PowerUp<float> _attackPowerPowerUp;       // 攻撃力強化データ
    private PowerUp<float> _defensePowerPowerUp;      // 防御力強化データ
    private PowerUp<float> _speedPowerUp;             // スピード強化データ
    private PowerUp<float> _dashCoolTimePowerUp;      // ダッシュクールタイム強化データ
    //public PowerUp<float> MpRecoverySpeedPowerUp;   // MP回復速度強化データ
    protected override void Awake()
    {
        base.Awake();
        InitValues();
    }
    
    /// <summary>
    /// 数値の初期
    /// </summary>
    public void InitValues()
    {
        #region VALUE

        _healthPowerUp = new PowerUp<float>
        {
            CurrentLevel = 1,
            MaxLevel = 5,
            BaseCost = 100,
            BaseInCreaseValue = 10,
            CostInCreaseRate = 1.2f,
            ValueDecreaseRate = 1f
        };
        
        _magicPointPowerUp = new PowerUp<float>
        {
            CurrentLevel = 0,
            MaxLevel = 5,
            BaseCost = 100,
            BaseInCreaseValue = 10,
            CostInCreaseRate = 1.2f,
            ValueDecreaseRate = 1f
        };
        
        _attackPowerPowerUp = new PowerUp<float>
        {
            CurrentLevel = 0,
            MaxLevel = 5,
            BaseCost = 100,
            BaseInCreaseValue = 1,
            CostInCreaseRate = 1.2f,
            ValueDecreaseRate = 1f
        };
        
        _defensePowerPowerUp = new PowerUp<float>
        {
            CurrentLevel = 0,
            MaxLevel = 5,
            BaseCost = 100,
            BaseInCreaseValue = 1,
            CostInCreaseRate = 1.2f,
            ValueDecreaseRate = 1f
        };
        
        _speedPowerUp = new PowerUp<float>
        {
            CurrentLevel = 0,
            MaxLevel = 5,
            BaseCost = 100,
            BaseInCreaseValue = 0.5f,
            CostInCreaseRate = 1.2f,
            ValueDecreaseRate = 1f
        };
        
        _dashCoolTimePowerUp = new PowerUp<float>
        {
            CurrentLevel = 0,
            MaxLevel = 5,
            BaseCost = 100,
            BaseInCreaseValue = 0.1f,
            CostInCreaseRate = 1.2f,
            ValueDecreaseRate = 1f
        };
        
        // MpRecoverySpeedPowerUp = new PowerUp<float>
        // {
        //     CurrentLevel = 0,
        //     MaxLevel = 5,
        //     BaseCost = 100,
        //     BaseInCreaseValue = 0.1f,
        //     CostInCreaseRate = 1.2f,
        //     ValueDecreaseRate = 1f
        // };

        #endregion
    }

    /// <summary>
    /// 保存された値を取得
    /// </summary>
    public void SetSavedValues(int healthLevel, int magicPointLevel, 
        int attackPowerLevel, int defensePowerLevel,int speedLevel,int dashCoolTimeLevel)
    {
        _healthPowerUp.CurrentLevel = healthLevel;
        _magicPointPowerUp.CurrentLevel = magicPointLevel;
        _attackPowerPowerUp.CurrentLevel = attackPowerLevel;
        _defensePowerPowerUp.CurrentLevel = defensePowerLevel;
        _speedPowerUp.CurrentLevel = speedLevel;
        _dashCoolTimePowerUp.CurrentLevel = dashCoolTimeLevel;
    }
    
    /// <summary>
    /// 強化を適用する
    /// </summary>
    /// <param name="stats">基本ステータス</param>
    /// <param name="playerSpecificStats">固有ステータス</param>
    public void ApplyValues(ref Stats stats, ref PlayerSpecificStats playerSpecificStats)
    {
        stats.Health += _healthPowerUp.GetValueIncrease(x => x);
        stats.MagicPoint += _magicPointPowerUp.GetValueIncrease(x => x);
        stats.AttackPower += _attackPowerPowerUp.GetValueIncrease(x => x);
        stats.DefensePower += _defensePowerPowerUp.GetValueIncrease(x => x);
        stats.Speed += _speedPowerUp.GetValueIncrease(x => x);
        playerSpecificStats.MpRecoverySpeed += _magicPointPowerUp.GetValueIncrease(x => x);
        playerSpecificStats.DashCoolTime -= _dashCoolTimePowerUp.GetValueIncrease(x => x);
    }

    #region UPGRADE
    
    /// <summary>
    /// HP強化
    /// </summary>
    /// <returns>強化できたか</returns>
    public bool UpgradeHealth()
    {
        int cost = _healthPowerUp.GetUpgradeCost();
        if (CoinSystem.Instance.Coin >= cost)
        {
            CoinSystem.Instance.UseCoin(cost);
            _healthPowerUp.Upgrade();
            return true;
        }
        return false;   
    }

    #endregion
}


/// <summary>
/// 強化データ
/// </summary>
public class PowerUp<T> where T : struct
{
    public int CurrentLevel = 0;             // 現在の強化レベル
    public int MaxLevel = 10;                // 最大強化レベル
    public int BaseCost;                     // 初期の強化コスト(消費コイン数)
    
    public T BaseInCreaseValue;              // 初期の強化値
    public float CostInCreaseRate;           // コスト増加率
    public float ValueDecreaseRate;          // 強化値減少率加率
    
    /// <summary>
    /// 強化に必要なコストを取得
    /// </summary>
    /// <returns>強化に必要なコスト</returns>
    public int GetUpgradeCost()
    {
        return Mathf.CeilToInt(BaseCost * Mathf.Pow(CostInCreaseRate,CurrentLevel));
    }
    
    /// <summary>
    /// ステータスの増加値を取得
    /// </summary>
    /// <param name="convert"></param>
    /// <returns></returns>
    public float GetValueIncrease(Func<T, float> convert)
    {
        if (Math.Abs(ValueDecreaseRate - 1) < 0.001f)
        {
            return convert(BaseInCreaseValue) * CurrentLevel;
        }
        else
        {
            return convert(BaseInCreaseValue) * Mathf.Pow(ValueDecreaseRate, CurrentLevel);
        }
    }
    
    // レベルアップ処理
    public void Upgrade()
    {
        if (CurrentLevel < MaxLevel)
        {
            CurrentLevel++;
            Debug.Log("現在のレベル:" + this.GetType().ToString() +CurrentLevel);
        }
    }
}
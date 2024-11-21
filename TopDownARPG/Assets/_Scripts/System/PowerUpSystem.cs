using System;
using FrameWork.Utils;
using UnityEngine;
using UnityEngine.Serialization;

public class PowerUpSystem : Singleton<PowerUpSystem>
{
    public PowerUp<float> _magicPointPowerUp { get; private set; }          // MP強化データ
    public PowerUp<float> _healthPowerUp { get; private set; }            // HP強化データ
    public PowerUp<float> _attackPowerPowerUp { get; private set; }         // 攻撃力強化データ
    public PowerUp<float> _defensePowerPowerUp { get; private set; }        // 防御力強化データ
    public PowerUp<float> _speedPowerUp { get; private set; }               // スピード強化データ
    public PowerUp<float> _dashCoolTimePowerUp { get; private set; }        // ダッシュクールタイム強化データ
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
            CurrentLevel = 0,
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
            BaseInCreaseValue = 2,
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
    /// <param name="status">基本ステータス</param>
    /// <param name="playerSpecificStatus">固有ステータス</param>
    public void ApplyValues(ref Status status, ref PlayerSpecificStatus playerSpecificStatus)
    {
        status.Health += _healthPowerUp.GetValueIncrease(x => x);
        status.MagicPoint += _magicPointPowerUp.GetValueIncrease(x => x);
        status.AttackPower += _attackPowerPowerUp.GetValueIncrease(x => x);
        status.DefensePower += _defensePowerPowerUp.GetValueIncrease(x => x);
        status.Speed += _speedPowerUp.GetValueIncrease(x => x);
        playerSpecificStatus.MpRecoverySpeed += _magicPointPowerUp.GetValueIncrease(x => x);
        playerSpecificStatus.DashCoolTime -= _dashCoolTimePowerUp.GetValueIncrease(x => x);
    }

    #region UPGRADE
    
    /// <summary>
    /// HP強化
    /// </summary>
    /// <returns>強化できたか</returns>
    public bool UpgradeHealth()
    {
        int cost = 100;
        
        if (CoinSystem.Instance.Coin >= cost && !_healthPowerUp.CheckMax() )
        {
            CoinSystem.Instance.UseCoin(cost);
            _healthPowerUp.Upgrade();
            return true;
        }
        return false;   
    }

    /// <summary>
    /// 攻撃力強化
    /// </summary>
    /// <returns>強化できたか</returns>
    public bool UpgradeAttack()
    {
        int cost = 100;

        if (CoinSystem.Instance.Coin >= cost && !_attackPowerPowerUp.CheckMax())
        {
            CoinSystem.Instance.UseCoin(cost);
            _attackPowerPowerUp.Upgrade();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 防御力強化
    /// </summary>
    /// <returns>強化できたか</returns>
    public bool UpgradeDefense()
    {
        int cost = 100;

        if (CoinSystem.Instance.Coin >= cost && !_defensePowerPowerUp.CheckMax())
        {
            CoinSystem.Instance.UseCoin(cost);
            _defensePowerPowerUp.Upgrade();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 移動速度強化
    /// </summary>
    /// <returns>強化できたか</returns>
    public bool UpgradeSpeed()
    {
        int cost = 100;

        if (CoinSystem.Instance.Coin >= cost && !_speedPowerUp.CheckMax())
        {
            CoinSystem.Instance.UseCoin(cost);
            _speedPowerUp.Upgrade();
            return true;
        }
        return false;
    }

    /// <summary>
    /// MP強化
    /// </summary>
    /// <returns>強化できたか</returns>
    public bool UpgradeMp()
    {
        int cost = 100;

        if (CoinSystem.Instance.Coin >= cost && !_magicPointPowerUp.CheckMax())
        {
            CoinSystem.Instance.UseCoin(cost);
            _magicPointPowerUp.Upgrade();
            return true;
        }
        return false;
    }

    /// <summary>
    /// ダッシュ強化
    /// </summary>
    /// <returns>強化できたか</returns>
    public bool UpgradeDash()
    {
        int cost = 100;

        if (CoinSystem.Instance.Coin >= cost && !_dashCoolTimePowerUp.CheckMax())
        {
            CoinSystem.Instance.UseCoin(cost);
            _dashCoolTimePowerUp.Upgrade();
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
    public int MaxLevel;                // 最大強化レベル
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

    public bool CheckMax()
    {
        if(CurrentLevel >= MaxLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;using FrameWork.Interface;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class LevelManager : MonoBehaviour,IInitializable
{
    private LevelDataBase _gameLevelDatabase;   //すべてのステージデータ
    
    private int _currentStageIndex = 0; //現在のステージインデックス
    private LevelData currentLevelData; //現在のステージデータ
    
    public void Init()
    {
        _gameLevelDatabase = ResManager.Instance.GetAssetCache<LevelDataBase>("LevelData/LevelData");
        if (!_gameLevelDatabase)
        {
            DebugLogger.Log("ステージデータが取得できません");
            return;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyManager"></param>
    public void SetWaveConfigToEnemyManager(EnemyManager enemyManager)
    {
        if(_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("ステージデータが取得できません");
            return;
        }
        currentLevelData = _gameLevelDatabase.StageDetails[_currentStageIndex];
        enemyManager.SetWaveConfig(currentLevelData.WaveConfigs);
    }
    
    /// <summary>
    /// レベルの更新
    /// </summary>
    public void UpdateLevel()
    {
        _currentStageIndex++;
        if(_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("ステージデータが取得できません");
            return;
        }
        
    }
}

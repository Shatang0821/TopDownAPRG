using System.Collections;
using System.Collections.Generic;using FrameWork.Interface;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class LevelManager : MonoBehaviour,IInitializable
{
    private LevelDataBase _gameLevelDatabase;   //���ׂẴX�e�[�W�f�[�^
    
    private int _currentStageIndex = 0; //���݂̃X�e�[�W�C���f�b�N�X
    private LevelData currentLevelData; //���݂̃X�e�[�W�f�[�^
    
    public void Init()
    {
        _gameLevelDatabase = ResManager.Instance.GetAssetCache<LevelDataBase>("LevelData/LevelData");
        if (!_gameLevelDatabase)
        {
            DebugLogger.Log("�X�e�[�W�f�[�^���擾�ł��܂���");
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
            DebugLogger.Log("�X�e�[�W�f�[�^���擾�ł��܂���");
            return;
        }
        currentLevelData = _gameLevelDatabase.StageDetails[_currentStageIndex];
        enemyManager.SetWaveConfig(currentLevelData.WaveConfigs);
    }
    
    /// <summary>
    /// ���x���̍X�V
    /// </summary>
    public void UpdateLevel()
    {
        _currentStageIndex++;
        if(_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("�X�e�[�W�f�[�^���擾�ł��܂���");
            return;
        }
        
    }
}

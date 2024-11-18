using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.Interface;
using FrameWork.Resource;
using FrameWork.UI;
using FrameWork.Utils;
using UnityEngine;

public class LevelManager : MonoBehaviour,IInitializable
{
    private LevelDataBase _gameLevelDatabase;   //���ׂẴX�e�[�W�f�[�^
    
    public int _currentStageIndex = 0; //���݂̃X�e�[�W�C���f�b�N�X
    private LevelData _currentLevelData; //���݂̃X�e�[�W�f�[�^
    private int _maxLevelNum = 0;       //�X�e�[�W��
    
    public GameObject CurrentStageInstance; // ���ݐ������ꂽ�X�e�[�W�I�u�W�F�N�g
    
    private EnemyManager _enemyManager; //�G�l�~�[�}�l�[�W���[
    private Player _player;             //�v���C���[
    
    //A*�A���S���Y��
    private AStar _aStar;               //�T���A���S���Y���C���X�^���X
    private int[,] _map;                //�X�e�[�W�f�[�^�z��
    
    public void Init()
    {
        _gameLevelDatabase = ResManager.Instance.GetAssetCache<LevelDataBase>("Config & Data/LevelDataBase/LevelDataBase");
        _aStar = new AStar();
        if (!_gameLevelDatabase)
        {
            DebugLogger.Log("�X�e�[�W�f�[�^���擾�ł��܂���");
            return;
        }
        
        _maxLevelNum = _gameLevelDatabase.StageDetails.Length;
        for(int i = 0;i < _maxLevelNum;i++)
        {
            _gameLevelDatabase.StageDetails[i].Initialize();
        }
    }

    private void OnDestroy()
    {
        if(CurrentStageInstance != null)
            Destroy(CurrentStageInstance);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
    
    /// <summary>
    /// �K�v�ȃp�����[�^��ݒ�
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemyManager"></param>
    public void SetParameters(Player player,EnemyManager enemyManager)
    {
        _player = player;
        _enemyManager = enemyManager;
    }
    
    /// <summary>
    /// ���x���̐���
    /// </summary>
    public void InitializeLevel()
    {
        //�Y�������ő�X�e�[�W���𒴂��Ȃ�
        if (_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("�X�e�[�W�f�[�^���擾�ł��܂���");
            return;
        }

        UpdateData();
    }

    public void UpdateData()
    {
        // ���݂̃X�e�[�W�f�[�^���擾
        _currentLevelData = _gameLevelDatabase.StageDetails[_currentStageIndex];

        // �X�e�[�W�I�u�W�F�N�g�̐���
        if (CurrentStageInstance != null)
        {
            Destroy(CurrentStageInstance);
        }

        CurrentStageInstance = _currentLevelData.SpawnLevel();
        _aStar.InitMap(_currentLevelData.Map);
    }
    
    /// <summary>
    /// �v���C���[�̐������E���W�̎擾
    /// </summary>
    /// <returns>�v���C���[�̐������E���W</returns>
    public Vector3 GetPlayerSpawnPos()
    { 
        if(_currentLevelData == null)
        {
            DebugLogger.Log("�X�e�[�W�f�[�^���擾�ł��܂���");
            return Vector3.zero;
        }
        
        var worldSpawnPos = CurrentStageInstance.transform.TransformPoint(_currentLevelData.PlayerSpawnPos);
        return worldSpawnPos;
    }
    
    

    /// <summary>
    /// ���݃��x���̃I�u�W�F�N�g�̎擾
    /// </summary>
    /// <returns>���x���I�u�W�F�N�g</returns>
    public GameObject GetCurrentStageInstance() => CurrentStageInstance;
    
    /// <summary>
    /// �G�l�~�[�}�l�[�W���[�ɃE�F�[�u�f�[�^��ݒ�
    /// </summary>
    /// <param name="enemyManager"></param>
    public void SetWaveConfigToEnemyManager(EnemyManager enemyManager)
    {
        _enemyManager.SetLevel(this.transform);
        
        if(_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("�X�e�[�W�f�[�^���擾�ł��܂���");
            return;
        }
        _currentLevelData = _gameLevelDatabase.StageDetails[_currentStageIndex];
        enemyManager.SetWaveConfig(_currentLevelData.WaveConfigs);
    }
    
    /// <summary>
    /// ���x���̍X�V
    /// </summary>
    public void UpdateLevel()
    {
        _currentStageIndex++;
        if (_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("�X�e�[�W�f�[�^���擾�ł��܂���");
            return;
        }
        CoinSystem.Instance.AddCoin(100);
        Debug.Log(CoinSystem.Instance.Coin);
        UpdateData();

        StartCoroutine(Remove());
        GameManager.Instance.PlayerManager.EnablePlayer(true);
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.RemoveUI("UIChange");
    }

    public void ResetLevel()
    {
        _currentStageIndex = 0;
        _currentLevelData = null;
        if (CurrentStageInstance != null)
        {
            Destroy(CurrentStageInstance);
        }
    }

    /// <summary>
    /// �p�X���擾����
    /// </summary>
    /// <param name="self">�����̈ʒu</param>
    /// <param name="target">�^�[�Q�b�g�̈ʒu</param>
    public List<AStar.AstarNode> FindPath(Vector3 self, Vector3 target)
    {
//        Debug.Log("start Pos" + self);
//        Debug.Log("target Pos" + target);
        Vector2Int startGrid = WorldToGridPosition(self);
        Vector2Int endGrid = WorldToGridPosition(target);
        
//        Debug.Log(startGrid +","+endGrid);
        var path = _aStar.FindPath(startGrid, endGrid);
        
        if (path != null)
        {
            return path;
        }
        Debug.LogWarning("Path not found.");
        return null;
    }
    
    /// <summary>
    /// ���[���h���W���O���b�h���W�ɕϊ�����
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        var stageConfig = _gameLevelDatabase.StageDetails[_currentStageIndex];
        
        // ���[���h���W�����[�J�����W�ɕϊ�
        Vector3 localPosition = CurrentStageInstance.transform.InverseTransformPoint(worldPosition);
        // ���[�J�����W����ɃO���b�h���W�ɕϊ�
        // ���[�J�����W����ɃO���b�h���W�ɕϊ�
        int x = Mathf.FloorToInt(Mathf.Abs((localPosition.x / stageConfig.CellSize)));
        int y = Mathf.FloorToInt(Mathf.Abs((localPosition.z / stageConfig.CellSize)));
        return new Vector2Int(x, y);
    }
    
    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        var stageConfig = _gameLevelDatabase.StageDetails[_currentStageIndex];
        // ���[�J�����W�ɕϊ�
        Vector3 localPosition = new Vector3(gridPosition.x * stageConfig.CellSize + stageConfig.CellSize / 2, 0, -gridPosition.y * stageConfig.CellSize - stageConfig.CellSize / 2);
    
        // �}�b�v��Transform���l�����ă��[���h���W�ɕϊ�
        return CurrentStageInstance.transform.TransformPoint(localPosition);
    }
}

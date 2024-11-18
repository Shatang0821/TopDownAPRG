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
    private LevelDataBase _gameLevelDatabase;   //すべてのステージデータ
    
    public int _currentStageIndex = 0; //現在のステージインデックス
    private LevelData _currentLevelData; //現在のステージデータ
    private int _maxLevelNum = 0;       //ステージ数
    
    public GameObject CurrentStageInstance; // 現在生成されたステージオブジェクト
    
    private EnemyManager _enemyManager; //エネミーマネージャー
    private Player _player;             //プレイヤー
    
    //A*アルゴリズム
    private AStar _aStar;               //探索アルゴリズムインスタンス
    private int[,] _map;                //ステージデータ配列
    
    public void Init()
    {
        _gameLevelDatabase = ResManager.Instance.GetAssetCache<LevelDataBase>("Config & Data/LevelDataBase/LevelDataBase");
        _aStar = new AStar();
        if (!_gameLevelDatabase)
        {
            DebugLogger.Log("ステージデータが取得できません");
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
    /// 必要なパラメータを設定
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemyManager"></param>
    public void SetParameters(Player player,EnemyManager enemyManager)
    {
        _player = player;
        _enemyManager = enemyManager;
    }
    
    /// <summary>
    /// レベルの生成
    /// </summary>
    public void InitializeLevel()
    {
        //添え字が最大ステージ数を超えない
        if (_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("ステージデータが取得できません");
            return;
        }

        UpdateData();
    }

    public void UpdateData()
    {
        // 現在のステージデータを取得
        _currentLevelData = _gameLevelDatabase.StageDetails[_currentStageIndex];

        // ステージオブジェクトの生成
        if (CurrentStageInstance != null)
        {
            Destroy(CurrentStageInstance);
        }

        CurrentStageInstance = _currentLevelData.SpawnLevel();
        _aStar.InitMap(_currentLevelData.Map);
    }
    
    /// <summary>
    /// プレイヤーの生成世界座標の取得
    /// </summary>
    /// <returns>プレイヤーの生成世界座標</returns>
    public Vector3 GetPlayerSpawnPos()
    { 
        if(_currentLevelData == null)
        {
            DebugLogger.Log("ステージデータが取得できません");
            return Vector3.zero;
        }
        
        var worldSpawnPos = CurrentStageInstance.transform.TransformPoint(_currentLevelData.PlayerSpawnPos);
        return worldSpawnPos;
    }
    
    

    /// <summary>
    /// 現在レベルのオブジェクトの取得
    /// </summary>
    /// <returns>レベルオブジェクト</returns>
    public GameObject GetCurrentStageInstance() => CurrentStageInstance;
    
    /// <summary>
    /// エネミーマネージャーにウェーブデータを設定
    /// </summary>
    /// <param name="enemyManager"></param>
    public void SetWaveConfigToEnemyManager(EnemyManager enemyManager)
    {
        _enemyManager.SetLevel(this.transform);
        
        if(_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("ステージデータが取得できません");
            return;
        }
        _currentLevelData = _gameLevelDatabase.StageDetails[_currentStageIndex];
        enemyManager.SetWaveConfig(_currentLevelData.WaveConfigs);
    }
    
    /// <summary>
    /// レベルの更新
    /// </summary>
    public void UpdateLevel()
    {
        _currentStageIndex++;
        if (_currentStageIndex >= _gameLevelDatabase.StageDetails.Length)
        {
            DebugLogger.Log("ステージデータが取得できません");
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
    /// パスを取得する
    /// </summary>
    /// <param name="self">自分の位置</param>
    /// <param name="target">ターゲットの位置</param>
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
    /// ワールド座標をグリッド座標に変換する
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        var stageConfig = _gameLevelDatabase.StageDetails[_currentStageIndex];
        
        // ワールド座標をローカル座標に変換
        Vector3 localPosition = CurrentStageInstance.transform.InverseTransformPoint(worldPosition);
        // ローカル座標を基にグリッド座標に変換
        // ローカル座標を基にグリッド座標に変換
        int x = Mathf.FloorToInt(Mathf.Abs((localPosition.x / stageConfig.CellSize)));
        int y = Mathf.FloorToInt(Mathf.Abs((localPosition.z / stageConfig.CellSize)));
        return new Vector2Int(x, y);
    }
    
    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        var stageConfig = _gameLevelDatabase.StageDetails[_currentStageIndex];
        // ローカル座標に変換
        Vector3 localPosition = new Vector3(gridPosition.x * stageConfig.CellSize + stageConfig.CellSize / 2, 0, -gridPosition.y * stageConfig.CellSize - stageConfig.CellSize / 2);
    
        // マップのTransformを考慮してワールド座標に変換
        return CurrentStageInstance.transform.TransformPoint(localPosition);
    }
}

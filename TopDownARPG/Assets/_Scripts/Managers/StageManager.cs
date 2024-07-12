using System.Collections.Generic;
using FrameWork.Utils;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private LevelDataBase _gameLevelDatabase;   //ゲームのすべてのステージデータ
    private int _currentStageIndex = 0; //現在ステージのインデクス
    private int _maxStageNum = 0;       //ステージ数
    private AStar _aStar;               //探索アルゴリズムインスタンス
    private int[,] _map;                //ステージデータ配列

    private GameObject _currentStagePrefab;//現在のステージオブジェクト
    public void Initialize(LevelDataBase levelDatabase)
    {
        _aStar = new AStar();
        _gameLevelDatabase = levelDatabase;
        _maxStageNum = _gameLevelDatabase.StageDetails.Length;
        Debug.Log("ステージロード前");
        for (int i = 0; i < _maxStageNum; i++)
        {
            _gameLevelDatabase.StageDetails[i].Initialize();
        }
        Debug.Log("ステージロード後");
    }

    /// <summary>
    /// ステージの変更
    /// </summary>
    /// <param name="index"></param>
    public void SetStage(int index)
    {
        //indexが最大ステージ数を超えない
        if (index > _maxStageNum) return;
        _currentStageIndex = index;
        CreateStage();
        
    }

    /// <summary>
    /// ステージ作成処理
    /// </summary>
    public void CreateStage()
    {
        //現在のステージが残っている場合削除する
        if (_currentStagePrefab)
        {
            GameObject.Destroy(_currentStagePrefab);
        }

        var stageConfig = _gameLevelDatabase.StageDetails[_currentStageIndex];
        // Y軸を-135度回転させてオブジェクトを生成
        Quaternion rotation = Quaternion.Euler(0, -135, 0);
        _currentStagePrefab =
            GameObject.Instantiate(stageConfig.StagePrefab, stageConfig.SpawnPos, rotation);
        
        _aStar.InitMap(stageConfig.Map);
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
        Vector3 localPosition = _currentStagePrefab.transform.InverseTransformPoint(worldPosition);
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
        return _currentStagePrefab.transform.TransformPoint(localPosition);
    }
}
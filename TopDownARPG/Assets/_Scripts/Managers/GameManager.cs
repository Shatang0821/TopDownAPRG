using System;
using FrameWork.Factories;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine.Serialization;

public enum GameState
{
    Title,
    Gameplay,
    GameOver
}

/// <summary>
/// ゲームプレイ全体の管理を行うクラスとメモリ管理
/// </summary>
public class GameManager : PersistentUnitySingleton<GameManager>
{
    //プレイヤーマネージャー
    private PlayerManager _playerManager;

    //エネミーマネージャー
    private EnemyManager _enemyManager;

    //レベルマネージャー
    private LevelManager _levelManager;


    [FormerlySerializedAs("stageDataBase")] public LevelDataBase levelDataBase;

    private void Start()
    {
        levelDataBase = ResManager.Instance.GetAssetCache<LevelDataBase>("StageDataBase/StageDataBase");
        
        StageManager.Instance.Initialize(levelDataBase);

        StageManager.Instance.SetStage(0);
        
        ChangeState(GameState.Gameplay);
    }

    private void Update()
    {
        if (_playerManager != null)
        {
            _playerManager.LogicUpdate();
        }

        if (_enemyManager != null)
        {
            _enemyManager.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (_playerManager != null)
        {
            _playerManager.PhysicsUpdate();
        }

        if (_enemyManager != null)
        {
            //_enemyManager.PhysicsUpdate();
        }
    }

    /// <summary>
    /// 状態の変更
    /// </summary>
    /// <param name="newState">変更状態</param>
    public void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Title:
                InitializeTitleState();
                break;
            case GameState.Gameplay:
                InitializeGameplayState();
                break;
            case GameState.GameOver:
                InitializeGameOverState();
                break;
        }
    }
    
    /// <summary>
    /// タイトル状態初期化
    /// </summary>
    private void InitializeTitleState()
    {
        DestroyManagers();
        // タイトル初期化ロジック
    }
    
    /// <summary>
    /// ゲームプレイ状態初期化
    /// </summary>
    private void InitializeGameplayState()
    {
        // 初始化需要的管理器
        _playerManager = ManagerFactory.Instance.CreateManager<PlayerManager>();
        _enemyManager = ManagerFactory.Instance.CreateManager<EnemyManager>();
        //カメラの設定
        CameraManager.Instance.SetFollowTarget(_playerManager.GetPlayerInstance().transform);
        
        _enemyManager.SetPlayerTransform(_playerManager.GetPlayerInstance().transform);
    }

    /// <summary>
    /// ゲームオーバー状態初期化
    /// </summary>
    private void InitializeGameOverState()
    {
        DestroyManagers();
    }
    
    /// <summary>
    /// ゲームプレイマネージャーを削除する
    /// </summary>
    private void DestroyManagers()
    {
        if (_playerManager != null)
        {
            Destroy(_playerManager.gameObject);
            _playerManager = null;
        }

        if (_enemyManager != null)
        {
            Destroy(_enemyManager.gameObject);
            _enemyManager = null;
        }
    }
}
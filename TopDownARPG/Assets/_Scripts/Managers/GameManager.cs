using System;
using System.Collections;
using FrameWork.Factories;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;
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
    public PlayerManager PlayerManager;

    //エネミーマネージャー
    public EnemyManager EnemyManager;

    //レベルマネージャー
    public LevelManager LevelManager;


    [FormerlySerializedAs("stageDataBase")] public LevelDataBase levelDataBase;
    protected override void Awake()
    {
        base.Awake();
        
    }

    private void  Start()
    {
        ChangeState(GameState.Title);
        //ChangeState(GameState.Gameplay);
    }

    private void Update()
    {
        if (PlayerManager != null)
        {
            PlayerManager.LogicUpdate();
        }

        if (EnemyManager != null)
        {
            EnemyManager.LogicUpdate();
        }
            
    }

    private void FixedUpdate()
    {
        if (PlayerManager != null)
        {
            PlayerManager.PhysicsUpdate();
        }

        if (EnemyManager != null)
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
        StartCoroutine(nameof(InitializeGameplayStateCoroutine));
    }
    
    private IEnumerator InitializeGameplayStateCoroutine()
    {
        // 必要なマネージャーの生成
        LevelManager = ManagerFactory.Instance.CreateManager<LevelManager>();
        LevelManager.InitializeLevel();
        
        PlayerManager = ManagerFactory.Instance.CreateManager<PlayerManager>();
        EnemyManager = ManagerFactory.Instance.CreateManager<EnemyManager>();
        

        // 必要なマネージャーの初期化
        
        EnemyManager.SetPlayerTransform(PlayerManager.GetPlayerInstance().transform);
        LevelManager.SetParameters(PlayerManager.GetPlayerInstance().GetComponent<Player>(),EnemyManager);
        LevelManager.SetWaveConfigToEnemyManager(EnemyManager);
        
        // カメラの設定
        CameraManager.Instance.Initialize();
        CameraManager.Instance.SetFollowTarget(PlayerManager.GetPlayerInstance().transform);
        
        
        yield break;
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
        if (PlayerManager != null)
        {
            Destroy(PlayerManager.gameObject);
            PlayerManager = null;
        }

        if (EnemyManager != null)
        {
            Destroy(EnemyManager.gameObject);
            EnemyManager = null;
        }

        if (LevelManager != null)
        {
            Destroy(LevelManager.gameObject);
            LevelManager = null;
        }
            
    }
}
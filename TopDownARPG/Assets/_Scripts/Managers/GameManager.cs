using FrameWork.Factories;
using FrameWork.Resource;
using FrameWork.Utils;

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


    public StageDataBase stageDataBase;

    private void Start()
    {
        stageDataBase = ResManager.Instance.GetAssetCache<StageDataBase>("StageDataBase/StageDataBase");
        EnemyManager.Instance.Initialize();
        StageManager.Instance.Initialize(stageDataBase);

        StageManager.Instance.SetStage(0);
        
        ChangeState(GameState.Gameplay);
    }

    void Update()
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
        ChangeState(GameState.GameOver);
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
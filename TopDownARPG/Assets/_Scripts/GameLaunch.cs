using FrameWork.Audio;
using FrameWork.Factories;
using FrameWork.Pool;
using FrameWork.Resource;
using FrameWork.UI;
using FrameWork.Utils;

public class GameLaunch : Singleton<GameLaunch>
{
    protected override void Awake()
    {
        base.Awake();
        this.InitFramework();
        this.InitGameLogic();
    }

    /// <summary>
    /// アップデートチェック
    /// </summary>
    private void CheckHotUpdate()
    {
        //データ取得
        //ダウンロード情報
        //ローカルにダウンロード
    }
    
    /// <summary>
    /// フレームワークを初期化
    /// </summary>
    private void InitFramework()
    {
        //ManagerFactory.Instance.CreateManager<AudioManager>();
        //ManagerFactory.Instance.CreateManager<PoolManager>();
    }

    /// <summary>
    /// ゲームロジックに入る
    /// </summary>
    private void InitGameLogic()
    {
        UIManager.Instance.ShowUI("UILogin");
        ManagerFactory.Instance.CreateManager<GameManager>();
    }
}

using FrameWork.Utils;
using UnityEngine;

public class GameApp : UnitySingleton<GameApp>
{
    public void InitGame()
    {
      Debug.Log("Enter Game!");  
      this.EnterMainScene();
    }
    /// <summary>
    /// ゲームシーンに入る
    /// </summary>
    private void EnterMainScene()
    {
        //マップの生成...
        /* string mapName = "Maps/Game.prefab";
         GameObject mapPrefab = ResManager.Instance.GetAssetCache<GameObject>(mapName);
         GameObject map = GameObject.Instantiate(mapPrefab);
         */
        //マネージャーの生成
        InitMgr();

        //UIの生成
        //UIManager.Instance.ShowUI("UIHome");

    }

    /// <summary>
    /// マネージャーのオブジェクトを生成
    /// </summary>
    private void InitMgr()
    {
        //ManagerFactory.Instance.CreateManager<PoolManager>(this.transform);
    }


}

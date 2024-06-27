    using FrameWork.Utils;
using UnityEngine;


public class GameManager : PersistentUnitySingleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        EnemyManager.Instance.Initialize();
    }
    
    
}
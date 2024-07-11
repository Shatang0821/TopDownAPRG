    using System;
    using FrameWork.Resource;
    using FrameWork.Utils;
using UnityEngine;
using UnityEngine.Serialization;


public class GameManager : PersistentUnitySingleton<GameManager>
{
    [HideInInspector]
    public StageDataBase stageDataBase;
    protected override void Awake()
    {
        base.Awake();
        stageDataBase = ResManager.Instance.GetAssetCache<StageDataBase>("StageDataBase/StageDataBase"); 
        EnemyManager.Instance.Initialize();
        StageManager.Instance.Initialize(stageDataBase);
    }

    private void Start()
    {
        StageManager.Instance.SetStage(0);
    }
}
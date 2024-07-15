using System.Collections.Generic;
using FrameWork.Utils;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private LevelDataBase _gameLevelDatabase;   //ゲームのすべてのステージデータ
    private int _currentStageIndex = 0; //現在ステージのインデクス
    private int _maxStageNum = 0;       //ステージ数


    private GameObject _currentStagePrefab;//現在のステージオブジェクト


    
    
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewStageConfig", menuName = "Stage/StageData", order = 1)]
public class LevelDataBase : ScriptableObject
{
    public LevelData[] StageDetails;
}

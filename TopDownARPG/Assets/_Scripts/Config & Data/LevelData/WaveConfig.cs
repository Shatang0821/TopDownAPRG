using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewEnemiesConfig", menuName = "EnemiesConfig", order = 1)]
public class WaveConfig : ScriptableObject 
{
    public List<EnemySpawnInfo> Enemies;
    public GameObject SpawnEffect;
}

[Serializable]
public class EnemySpawnInfo
{
    public GameObject EnemyPrefab;      //敵のプレハブ
    public Vector3 SpawnLocalPosition;  //ステージの相対位置 変換が必要
    public float SpawnTime;             //生成時間
}
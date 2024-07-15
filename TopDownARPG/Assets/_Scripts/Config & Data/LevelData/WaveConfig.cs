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
    public GameObject EnemyPrefab;      //�G�̃v���n�u
    public Vector3 SpawnLocalPosition;  //�X�e�[�W�̑��Έʒu �ϊ����K�v
    public float SpawnTime;             //��������
}
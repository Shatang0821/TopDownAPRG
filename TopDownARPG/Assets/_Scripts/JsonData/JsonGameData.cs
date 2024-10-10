using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonGameData
{
    public PlayerData player; // �v���C���[�f�[�^
    public List<EnemyData> Enemy; // �G�f�[�^
    public string result; // ���ʃ��b�Z�[�W
}

[System.Serializable]
public class PlayerData
{
    public int id;
    public int maxhealth;
    public int defense;
    public int power;
    public int speed; 
    public int mp;
    public int dashcooltime;
    public int coin;
}

[System.Serializable]
public class EnemyData
{
    public int id;
    public string name;
    public int health;
    public int power;
    public float speed; 
}
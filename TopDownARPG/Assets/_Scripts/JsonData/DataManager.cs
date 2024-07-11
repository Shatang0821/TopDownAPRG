using FrameWork.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : PersistentUnitySingleton<DataManager>
{
    public int id { get; private set; } //�v���C���[��id
    public int maxhealth { get; private set; } //�v���C���[�̍ő�HP
    public int currenthealth { get; private set; } //�v���C���[�̌��݂�HP
    public int power { get; private set; } //�v���C���[�̍U����
    public float speed { get; private set; } //�v���C���[�̑��x
    public int mp { get; private set; } //�v���C���[��MP


    public void GameDataSet(JsonGameData playerData)
    {
        // GameData�Ƀf�[�^���Z�b�g
        GameData gameData = new GameData();
        gameData.SetBasicData(playerData);

        Debug.Log(gameData.ToString());

        id = gameData.p_id;
        maxhealth = gameData.p_maxhealth;
        currenthealth = gameData.p_currenthealth;
        power = gameData.p_power;
        speed = gameData.p_speed;
        mp = gameData.p_mp;
    }
}

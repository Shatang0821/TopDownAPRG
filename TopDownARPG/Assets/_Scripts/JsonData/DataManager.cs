using FrameWork.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : PersistentUnitySingleton<DataManager>
{
    public int id { get; private set; } //�v���C���[��id
    public int maxhealth { get; private set; } //�v���C���[�̍ő�HP
    public int defense { get; private set; } //�v���C���[�̖h���
    public int power { get; private set; } //�v���C���[�̍U����
    public int speed { get; private set; } //�v���C���[�̑��x
    public int mp { get; private set; } //�v���C���[��MP
    public int dashcooltime { get; private set; } //�v���C���[�̃_�b�V���̃N�[���^�C��
    public int coin { get; private set; } //�v���C���[�̃R�C����


    public void GameDataSet(JsonGameData playerData)
    {
        // GameData�Ƀf�[�^���Z�b�g
        GameData gameData = new GameData();
        gameData.SetBasicData(playerData);

        Debug.Log(gameData.ToString());

        id = gameData.p_id;
        maxhealth = gameData.p_maxhealth;
        defense = gameData.p_defense;
        power = gameData.p_power;
        speed = gameData.p_speed;
        mp = gameData.p_mp;
        dashcooltime = gameData.p_dashcooltime;
        coin = gameData.p_coin;
    }

    
}

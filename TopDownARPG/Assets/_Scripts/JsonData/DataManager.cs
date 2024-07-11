using FrameWork.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : PersistentUnitySingleton<DataManager>
{
    public int id { get; private set; } //プレイヤーのid
    public int maxhealth { get; private set; } //プレイヤーの最大HP
    public int currenthealth { get; private set; } //プレイヤーの現在のHP
    public int power { get; private set; } //プレイヤーの攻撃力
    public float speed { get; private set; } //プレイヤーの速度
    public int mp { get; private set; } //プレイヤーのMP


    public void GameDataSet(JsonGameData playerData)
    {
        // GameDataにデータをセット
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

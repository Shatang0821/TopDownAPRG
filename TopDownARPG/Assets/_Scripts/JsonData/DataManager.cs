using FrameWork.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : PersistentUnitySingleton<DataManager>
{
    public int id { get; private set; } //プレイヤーのid
    public int maxhealth { get; private set; } //プレイヤーの最大HP
    public int defense { get; private set; } //プレイヤーの防御力
    public int power { get; private set; } //プレイヤーの攻撃力
    public int speed { get; private set; } //プレイヤーの速度
    public int mp { get; private set; } //プレイヤーのMP
    public int dashcooltime { get; private set; } //プレイヤーのダッシュのクールタイム
    public int coin { get; private set; } //プレイヤーのコイン数


    public void GameDataSet(JsonGameData playerData)
    {
        // GameDataにデータをセット
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

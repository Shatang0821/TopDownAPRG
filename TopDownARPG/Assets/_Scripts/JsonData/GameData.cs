using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int p_id;
    public int p_maxhealth;
    public int p_defense;
    public int p_power;
    public int p_speed; // speedÇÕé¿ç€Ç…ÇÕfloatå^Ç≈Ç†ÇÈÇ±Ç∆Ç…íçà”
    public int p_mp;
    public int p_dashcooltime;
    public int p_coin;

    public void SetBasicData(JsonGameData basic)
    {
        p_id = basic.player.id;
        p_maxhealth = basic.player.maxhealth;
        p_defense = basic.player.defense;
        p_power = basic.player.power;
        p_speed = basic.player.speed;
        p_mp = basic.player.mp;
        p_dashcooltime = basic.player.dashcooltime;
        p_coin = basic.player.coin;
    }
    public override string ToString()
    {
        return $"ID: {p_id}, Max Health: {p_maxhealth}, Defense: {p_defense}, Power: {p_power}, Speed: {p_speed}, MP: {p_mp},DashCoolTime:{p_dashcooltime},Coin:{p_coin}";
    }
}
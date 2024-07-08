using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int p_id;
    public int p_maxhealth;
    public int p_currenthealth;
    public int p_power;
    public float p_speed; // speedÇÕé¿ç€Ç…ÇÕfloatå^Ç≈Ç†ÇÈÇ±Ç∆Ç…íçà”
    public int p_mp;

    public void SetBasicData(JsonGameData basic)
    {
        p_id = basic.player.id;
        p_maxhealth = basic.player.maxhealth;
        p_currenthealth = basic.player.currenthealth;
        p_power = basic.player.power;
        p_speed = basic.player.speed;
        p_mp = basic.player.mp;
    }
    public override string ToString()
    {
        return $"ID: {p_id}, Max Health: {p_maxhealth}, Current Health: {p_currenthealth}, Power: {p_power}, Speed: {p_speed}, MP: {p_mp}";
    }
}
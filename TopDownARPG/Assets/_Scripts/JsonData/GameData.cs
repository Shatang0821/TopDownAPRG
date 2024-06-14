using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int id;
    public int maxhealth;
    public int currenthealth;
    public int power;
    public float speed; // speedÇÕé¿ç€Ç…ÇÕfloatå^Ç≈Ç†ÇÈÇ±Ç∆Ç…íçà”
    public int mp;

    public void SetBasicData(JsonGameData basic)
    {
        id = basic.player.id;
        maxhealth = basic.player.maxhealth;
        currenthealth = basic.player.currenthealth;
        power = basic.player.power;
        speed = basic.player.speed;
        mp = basic.player.mp;
    }
    public override string ToString()
    {
        return $"ID: {id}, Max Health: {maxhealth}, Current Health: {currenthealth}, Power: {power}, Speed: {speed}, MP: {mp}";
    }
}
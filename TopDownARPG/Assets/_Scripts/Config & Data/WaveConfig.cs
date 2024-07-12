using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemiesConfig", menuName = "EnemiesConfig", order = 1)]
public class WaveConfig : ScriptableObject 
{
    public List<GameObject> Enemies;
}
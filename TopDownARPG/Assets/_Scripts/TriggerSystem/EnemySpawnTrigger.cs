using System.Collections;
using System.Collections.Generic;
using FrameWork.EventCenter;
using UnityEngine;

public class EnemySpawnTrigger : TriggerReceiver
{
    
    public override void OnTriggerReceived()
    {
        Debug.Log("EnemySpawnTrigger received trigger. Starting wave.");
        GameManager.Instance.EnemyManager.StartWave();
    }
    
    
}

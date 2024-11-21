using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.EventCenter;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("EnemySpawnTrigger received trigger. Starting wave.");
            GameManager.Instance.EnemyManager.StartWave();
        }

        gameObject.SetActive(false);
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FrameWork.UI;
using UnityEngine;

public class LevelChangeTrigger : MonoBehaviour
{
    LevelManager _levelManager;

    private void OnTriggerEnter(Collider other)
    {
        UIManager.Instance.ShowUI("UIChange");

        StartCoroutine(ChangeStage());
    }

    IEnumerator ChangeStage()
    {
        GameManager.Instance.PlayerManager.EnablePlayer(false);
        yield return new WaitForSeconds(2);
        var _levelM = GameManager.Instance.LevelManager;
        _levelM.UpdateLevel();
        GameManager.Instance.PlayerManager.UpdatePlayerPosition(_levelM.GetPlayerSpawnPos());
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FrameWork.UI;
using UnityEngine;

public class LevelChangeReceiver : TriggerReceiver
{
    LevelManager _levelManager;
    public override void OnTriggerReceived()
    {
        base.OnTriggerReceived();
        UIManager.Instance.ShowUI("UIChange");
        StartCoroutine(ChangeStage());

    }

    IEnumerator ChangeStage()
    {
        yield return new WaitForSeconds(3);
        var _levelM = GameManager.Instance.LevelManager;
        _levelM.UpdateLevel();
        Debug.Log(_levelM.GetPlayerSpawnPos());
        GameManager.Instance.PlayerManager.UpdatePlayerPosition(_levelM.GetPlayerSpawnPos());

        UIManager.Instance.RemoveUI("UIChange");
    }



}

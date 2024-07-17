using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FrameWork.UI;
using UnityEngine;

public class LevelClearReceiver : TriggerReceiver
{
    public override void OnTriggerReceived()
    {
        base.OnTriggerReceived();
        
        UIManager.Instance.RemoveUI("UIGame");
        UIManager.Instance.ShowUI("UIWin");
        GameManager.Instance.ChangeState(GameState.GameOver);
    }
}

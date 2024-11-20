using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FrameWork.UI;
using UnityEngine;

public class LevelClearTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.RemoveUI("UIGame");
            UIManager.Instance.ShowUI("UIWin");
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }
}

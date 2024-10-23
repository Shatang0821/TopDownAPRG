/*
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIHomeCtrl : UICtrl
{
    private List<Selectable> _homeSelectables = new List<Selectable>(); // 存储可选择的UI元素（如按钮）的列表
    private List<Selectable> _setSelectables = new List<Selectable>();
    private List<Selectable> _upSelectables = new List<Selectable>();
    private List<Selectable> _currentSelectables; // 当前活动的可选择元素列表
    private int _currentSelectionIndex = 0; // 当前选择的元素索引
    private Button[] buttons; // 用于存储所有按钮的数组

    public override void Awake()
    {
        base.Awake();

        // 初始化登录界面的可选择元素
        _homeSelectables.Add(View["GameStart"].GetComponent<Button>());
        _homeSelectables.Add(View["Settings"].GetComponent<Button>());
        _homeSelectables.Add(View["Operation"].GetComponent<Button>());
        _homeSelectables.Add(View["UpDate"].GetComponent<Button>());
        _homeSelectables.Add(View["Exit"].GetComponent<Button>());
        _homeSelectables.Add(View["Logout"].GetComponent<Button>());
        _currentSelectables = _homeSelectables;

        _setSelectables.Add(View["SettingsPanel/BGMSlider"].GetComponent<Slider>());
        _setSelectables.Add(View["SettingsPanel/GSESlider"].GetComponent<Slider>());

        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/HEALTH/HEALTH"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/ATTACK/ATTACK"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/DEFENSE/DEFENSE"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/SPEED/SPEED"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/MP/MP"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/DASH/DASH"].GetComponent<Button>());

        // 添加按钮事件
        AddButtonListener("GameStart", GameStart);
        AddButtonListener("Settings", Settings);
        AddButtonListener("Operation", Operation);
        AddButtonListener("UpDate", UpDate);
        AddButtonListener("Exit", Exit);
        AddButtonListener("Logout", Logout);

        AddButtonListener("UIUpDate/UpDatePanel/status/HEALTH/HEALTH", HEALTH);
        AddButtonListener("UIUpDate/UpDatePanel/status/ATTACK/ATTACK", ATTACK);
        AddButtonListener("UIUpDate/UpDatePanel/status/DEFENSE/DEFENSE", DEFENSE);
        AddButtonListener("UIUpDate/UpDatePanel/status/SPEED/SPEED", SPEED);
        AddButtonListener("UIUpDate/UpDatePanel/status/MP/MP", MP);
        AddButtonListener("UIUpDate/UpDatePanel/status/DASH/DASH", DASH);
    }

    // 选择上一个可选择的UI元素
    private void SelectPreviousInput()
    {
        // 如果当前选择的是按钮，取消其选中状态
        var current = _currentSelectables[_currentSelectionIndex];
        if (current is Button button)
        {
            var hoverEffect = button.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnDeselect(null); // 手动取消选中
            }
        }

        // 更新选择索引
        _currentSelectionIndex--;
        if (_currentSelectionIndex < 0)
        {
            _currentSelectionIndex = _currentSelectables.Count - 1;
        }

        // 选择新的UI元素
        _currentSelectables[_currentSelectionIndex].Select();

        // 如果新的选择是按钮，手动选中它
        var newCurrent = _currentSelectables[_currentSelectionIndex];
        if (newCurrent is Button newButton)
        {
            var hoverEffect = newButton.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnSelect(null); // 手动选中
            }
        }
    }

    // 选择下一个可选择的UI元素
    private void SelectNextInput()
    {
        // 如果当前选择的是按钮，取消其选中状态
        var current = _currentSelectables[_currentSelectionIndex];
        if (current is Button button)
        {
            var hoverEffect = button.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnDeselect(null); // 手动取消选中
            }
        }

        // 更新选择索引
        _currentSelectionIndex++;
        if (_currentSelectionIndex >= _currentSelectables.Count)
        {
            _currentSelectionIndex = 0;
        }

        // 选择新的UI元素
        _currentSelectables[_currentSelectionIndex].Select();

        // 如果新的选择是按钮，手动选中它
        var newCurrent = _currentSelectables[_currentSelectionIndex];
        if (newCurrent is Button newButton)
        {
            var hoverEffect = newButton.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnSelect(null); // 手动选中
            }
        }
    }

#if true
    // ゲームを開始する処理
    private void GameStart()
    {
        Debug.Log("GameStart");

    }

    // 設定画面を表示する処理
    private void Settings()
    {
        Debug.Log("Settings");

    }

    // 操作説明画面を表示する処理
    private void Operation()
    {
        Debug.Log("Operation");

    }

    // パワーアップ画面を表示する処理
    private void UpDate()
    {
        Debug.Log("UIUpDate");

    }

    // アプリケーションを終了する処理
    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void Logout()
    {

    }

    private void HEALTH()
    {
        Debug.Log("Exit");

    }
    private void ATTACK()
    {
        Debug.Log("Exit");
    }
    private void DEFENSE()
    {
        Debug.Log("Exit");
    }
    private void SPEED()
    {
        Debug.Log("Exit");
    }
    private void MP()
    {
        Debug.Log("Exit");
    }
    private void DASH()
    {
        Debug.Log("Exit");
    }
#endif
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar_HUD : StatsManager
{
    private float maxValue = 1f;  // 假设最大血量是1
    private float currentValue;

    private void Start()
    {
        // 初始设置当前值
        currentValue = maxValue;

        Initialize(currentValue, maxValue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed");  // 添加调试信息
            DecreaseHealth(0.1f);  // 按下空格键减少0.1的血量
        }
    }

    private void DecreaseHealth(float amount)
    {
        currentValue -= amount;
        if (currentValue < 0) currentValue = 0;
        Debug.Log("Current Health: " + currentValue);  // 添加调试信息
        UpdateStats(currentValue, maxValue);
    }
}

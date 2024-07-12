using FrameWork.EventCenter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private float maxValue = 100f;  // 假设最大血量是100
    private float currentValue;

    private Transform cameraTransform;

    private void OnEnable()
    {
        EventCenter.AddListener<float>(HPBar_EVENT.Change, DecreaseHealth);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<float>(HPBar_EVENT.Change, DecreaseHealth);
    }

    private void Start()
    {
        // 初始设置当前值
        currentValue = maxValue;
        Initialize(currentValue, maxValue);

        // 找到主摄像机
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // 使血条面向摄像机
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                         cameraTransform.rotation * Vector3.up);
    }

    private void DecreaseHealth(float newValue)
    {
        currentValue = newValue;
        if (currentValue < 0) currentValue = 0;
        Debug.Log("Current Health: " + currentValue);  // 添加调试信息
        UpdateStats(currentValue, maxValue);
    }

    private void Initialize(float currentValue, float maxValue)
    {
        // 初始化血条的方法（根据你的实现）
    }

    private void UpdateStats(float currentValue, float maxValue)
    {
        // 更新血条的方法（根据你的实现）
    }
}

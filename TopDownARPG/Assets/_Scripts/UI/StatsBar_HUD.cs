using FrameWork.EventCenter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum HPBar_EVENT
{
    Change
}

public class StatsBar_HUD : StatsManager
{
    private float maxValue = 100f;
    private float currentValue;
    
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
        maxValue = GameObject.FindWithTag("Player").GetComponent<Player>().Health;
        currentValue = maxValue;

        Initialize(currentValue, maxValue);
    }

    private void DecreaseHealth(float newValue)
    {
        currentValue = newValue;
        if (currentValue < 0) currentValue = 0;
        UpdateStats(currentValue, maxValue);
    }
}

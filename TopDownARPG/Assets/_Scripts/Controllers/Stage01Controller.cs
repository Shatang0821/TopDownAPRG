using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FrameWork.EventCenter;
using UnityEngine;

enum StageEventEnum
{
    EnterArea1,
    EnterArea2,
}

public class Stage01Controller : MonoBehaviour
{
    public TriggerReceiver Door1;
    public TriggerReceiver Door2;

    public CinemachineVirtualCamera Door01virtualCamera;
    public CinemachineVirtualCamera Door02virtualCamera;

    public TriggerReceiver Area1;
    
    private int _count = 0;

    private void OnEnable()
    {
        EventCenter.AddListener(EnemyEventEnum.OnWaveClear, WaveClear);
        EventCenter.AddListener(StageEventEnum.EnterArea1, OnEnterArea1);
        EventCenter.AddListener(StageEventEnum.EnterArea2, OnEnterArea2);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener(EnemyEventEnum.OnWaveClear, WaveClear);
        EventCenter.RemoveListener(StageEventEnum.EnterArea1, OnEnterArea1);
        EventCenter.RemoveListener(StageEventEnum.EnterArea2, OnEnterArea2);
    }

    private void WaveClear()
    {
        if (_count == 0)
        {
            OnWave1Complete();
        }
        else if (_count == 1)
        {
            OnWave2Complete();
        }

        _count++;
    }

    private void OnWave1Complete()
    {
        Door01virtualCamera.Priority = 6;
        Door1.OnTriggerReceived();
        StartCoroutine(ResetCamera(Door01virtualCamera, 3f));
    }

    private void OnWave2Complete()
    {
        Door02virtualCamera.Priority = 6;
        Door1.OnTriggerReceived();
        Door2.OnTriggerReceived();
        StartCoroutine(ResetCamera(Door02virtualCamera, 3f));
    }

    private void OnEnterArea1()
    {
        //ÉGÉäÉAà íuÇÃìGê∂ê¨
        EventCenter.TriggerEvent(EnemyEventEnum.OnStartWave);
    }

    private void OnEnterArea2()
    {
        EventCenter.TriggerEvent(EnemyEventEnum.OnStartWave);
    }

    private IEnumerator ResetCamera(CinemachineVirtualCamera camera, float delay)
    {
        yield return new WaitForSeconds(delay);
        camera.Priority = 0;
    }
}
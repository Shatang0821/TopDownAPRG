using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FrameWork.Utils;
using UnityEngine;

public class CameraManager : UnitySingleton<CameraManager>
{
    CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    
    public void SetFollowTarget(Transform target)
    {
        _virtualCamera.Follow = target;
    }
}

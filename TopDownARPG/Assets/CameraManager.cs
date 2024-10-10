using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FrameWork.Utils;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    CinemachineVirtualCamera _virtualCamera;

    public void Initialize()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    
    public void SetFollowTarget(Transform target)
    {
        _virtualCamera.Follow = target;
    }
}

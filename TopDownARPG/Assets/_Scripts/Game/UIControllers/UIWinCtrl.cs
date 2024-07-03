using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;

public class UIWinCtrl : UICtrl
{
    public override void Awake()
    {
        base.Awake();

        AudioManager.Instance.PlayWinBgm();
        AudioManager.Instance.StopAllNonWinBgms();
    }

    void Start()
    {
        //PlayWinBgm();
    }


}

using UnityEngine;
using FrameWork.UI;
using FrameWork.Audio;

public class UIWinCtrl : UICtrl
{
    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        //PlayWinBgm();
    }

  /*  private void PlayWinBgm()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWinBgm();
        }
        else
        {
            Debug.LogError("AudioManager instance is null. Cannot play win BGM.");
        }
    }
  */
}

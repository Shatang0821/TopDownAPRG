using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;


public class UIGameCtrl : UICtrl 
{
    private Image _blackImage; // Black Image を格納するための変数

    public override void Awake() {

		base.Awake();
        _blackImage = View["Black"].GetComponent<Image>();// Black Image を取得

        // 1秒後にBlack Imageを非表示にするコルーチンを開始
        StartCoroutine(HideBlackImageAfterDelay());
    }

    // 1秒後にBlack Imageを非表示にするコルーチン
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }



    void Start() {
	}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            UIManager.Instance.RemoveUI("UIGame");
            UIManager.Instance.ShowUI("UIEnd");
            UIManager.Instance.ChangeUIPrefab("UIEnd");

            if (this.gameObject != null)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

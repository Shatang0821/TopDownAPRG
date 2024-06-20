using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;


public class UIGameCtrl : UICtrl 
{
    private Image _blackImage; // Black Image ���i�[���邽�߂̕ϐ�

    public override void Awake() {

		base.Awake();
        _blackImage = View["Black"].GetComponent<Image>();// Black Image ���擾

        // 1�b���Black Image���\���ɂ���R���[�`�����J�n
        StartCoroutine(HideBlackImageAfterDelay());
    }

    // 1�b���Black Image���\���ɂ���R���[�`��
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }



    void Start() {
	}

}

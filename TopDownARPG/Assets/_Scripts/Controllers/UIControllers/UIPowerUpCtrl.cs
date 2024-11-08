using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using System;

public class UIPowerUpCtrl : UICtrl
{
    private Image _blackImage; // Black Image ���i�[���邽�߂̕ϐ�

    [SerializeField]
    private PowerUpData _powerUpData; // �p���[�A�b�v�f�[�^�x�[�X

    public override void Awake()
    {
        base.Awake();
        _blackImage = View["Black"].GetComponent<Image>(); // Black Image ���擾

        // 1�b���Black Image���\���ɂ���R���[�`�����J�n
        StartCoroutine(HideBlackImageAfterDelay());

        Debug.Log("Power-Up Name: " + _powerUpData.powerUpName);
        Debug.Log("Description: " + _powerUpData.description);
    }

    // 1�b���Black Image���\���ɂ���R���[�`��
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }
}

[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData", order = 1)]
public class PowerUpData : ScriptableObject
{
    public string powerUpName; // �p���[�A�b�v�̖��O
    public string description; // �p���[�A�b�v�̏ڍ�
}

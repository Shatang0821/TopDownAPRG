using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using System;

public class UIPowerUpCtrl : MonoBehaviour
{
    
    [SerializeField]
    private Text _cointext;
    [SerializeField]
    private Image _blackImage; // Black Image ���i�[���邽�߂̕ϐ�
    public void Update()
    {
        _cointext.text = "Coin:" + CoinSystem.Instance.Coin;
    }
}


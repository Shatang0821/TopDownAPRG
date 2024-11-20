using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class UIPowerUpCtrl : MonoBehaviour
{
    
    [SerializeField]
    private Text _cointext;
    [SerializeField]
    private Image _blackImage; // Black Image Çäiî[Ç∑ÇÈÇΩÇﬂÇÃïœêî
    [SerializeField]
    private GameObject[] _healthLevel;
    [SerializeField]
    private GameObject[] _attackLevel;
    [SerializeField]
    private GameObject[] _defenseLevel;
    [SerializeField]
    private GameObject[] _speedLevel;
    [SerializeField]
    private GameObject[] _mpLevel;
    [SerializeField]
    private GameObject[] _dashLevel;
    public void Update()
    {
        _cointext.text = "Coin:" + CoinSystem.Instance.Coin;
        for(int i = 0;i < PowerUpSystem.Instance._healthPowerUp.CurrentLevel;i++)
        {
            _healthLevel[i].SetActive(true);
        }

        for (int i = 0; i < PowerUpSystem.Instance._attackPowerPowerUp.CurrentLevel; i++)
        {
            _attackLevel[i].SetActive(true);
        }

        for (int i = 0; i < PowerUpSystem.Instance._defensePowerPowerUp.CurrentLevel; i++)
        {
            _defenseLevel[i].SetActive(true);
        }

        for (int i = 0; i < PowerUpSystem.Instance._speedPowerUp.CurrentLevel; i++)
        {
            _speedLevel[i].SetActive(true);
        }

        for (int i = 0; i < PowerUpSystem.Instance._magicPointPowerUp.CurrentLevel; i++)
        {
            _mpLevel[i].SetActive(true);
        }

        for (int i = 0; i < PowerUpSystem.Instance._dashCoolTimePowerUp.CurrentLevel; i++)
        {
            _dashLevel[i].SetActive(true);
        }
    }
}


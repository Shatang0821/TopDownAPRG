using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXManager : MonoBehaviour
{
    public ParticleSystem SlashVFX01;     //攻撃エフェクト
    public ParticleSystem SlashVFX02;     //攻撃エフェクト
    public ParticleSystem SlashVFX03;     //攻撃エフェクト
    public void PlaySlashVFX01()
    {
        SlashVFX01.Play();
    }
    public void PlaySlashVFX02()
    {
        SlashVFX02.Play();
    }
    public void PlaySlashVFX03()
    {
        SlashVFX03.Play();
    }
}

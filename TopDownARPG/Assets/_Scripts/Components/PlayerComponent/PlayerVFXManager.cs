using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXManager : MonoBehaviour
{
    public ParticleSystem SlashVFX01;     //�U���G�t�F�N�g
    public ParticleSystem SlashVFX02;     //�U���G�t�F�N�g
    public ParticleSystem SlashVFX03;     //�U���G�t�F�N�g
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

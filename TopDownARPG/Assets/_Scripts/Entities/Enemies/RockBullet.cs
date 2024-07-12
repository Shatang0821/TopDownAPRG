using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class RockBullet : MonoBehaviour
{
    GameObject _player;     // �v���C���[�I�u�W�F�N�g
    Rigidbody _rb;          // ��� Rigidbody �R���|�[�l���g

    public float moveSpeed = 10f; // ��̈ړ����x
    Vector3 directionToPlayer;

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        // �v���C���[�̈ʒu��ڕW�Ƃ�������x�N�g�����v�Z
        directionToPlayer = (_player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;
    }



    void Update()
    {
        if (_player == null)
        {
            return;
        }


        // ��ɑ��x��^���ăv���C���[�̕����ɔ�΂�
        _rb.velocity = directionToPlayer * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableEntity = other.GetComponent<Collider>().GetComponent<IDamageable>();

        if (damageableEntity != null)
        {
            damageableEntity.TakeDamage(5);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }


    }
}

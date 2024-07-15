using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class RockBullet : MonoBehaviour
{
    GameObject _player;     
    Rigidbody _rb;          

    public float moveSpeed = 10f; 
    Vector3 directionToPlayer;

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player == null)
        {
            return;
        }
        _rb = GetComponent<Rigidbody>();
        directionToPlayer = (_player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;
    }



    void Update()
    {
        if (_player == null)
        {
            return;
        }

        
        _rb.velocity = directionToPlayer * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableEntity = other.GetComponent<Collider>().GetComponent<IDamageable>();

        if (damageableEntity != null)
        {
            damageableEntity.TakeDamage(5);
            Destroy(this.gameObject);
            //gameObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
            //gameObject.SetActive(false);
        }


    }
}

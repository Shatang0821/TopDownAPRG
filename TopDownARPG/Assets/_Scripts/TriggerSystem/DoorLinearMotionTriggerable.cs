using System;
using System.Collections;
using UnityEngine;

public class DoorLinearMotionTriggerable : TriggerReceiver
{
    [SerializeField] private Vector3 _motionIncrement;
    [SerializeField] private float _duration;
    [SerializeField] private float _delay;
    private bool _isOpen = false;

    public override void OnTriggerReceived()
    {
        if(_isOpen)
        {
            StartCoroutine(CloseDooreCoroutine());
        }
        else
        {
            StartCoroutine(OpenDoorCoroutine());
        }
    }

    private IEnumerator OpenDoorCoroutine()
    {
        yield return new WaitForSeconds(_delay);

        float moveTime = 0f;

        while (moveTime < _duration)
        {
            transform.Translate(_motionIncrement * Time.deltaTime);
            moveTime += Time.deltaTime;

            _isOpen = true;
            yield return null;
        }
    }

    private IEnumerator CloseDooreCoroutine()
    {
        float moveTime = 0f;

        while (moveTime < _duration)
        {
            transform.Translate(-1 * _motionIncrement * Time.deltaTime);
            moveTime += Time.deltaTime;

            _isOpen = false;
            yield return null;
        }
    }
}
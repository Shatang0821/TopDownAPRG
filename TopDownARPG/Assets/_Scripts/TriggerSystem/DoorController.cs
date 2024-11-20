using System.Collections;
using FrameWork.EventCenter;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject Door;
    [SerializeField] private Vector3 _motionIncrement;
    [SerializeField] private float _duration;
    private void OnEnable()
    {
        EventCenter.AddListener(LevelEvent.Clear,TestExcute);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener(LevelEvent.Clear,TestExcute);
    }

    private void TestExcute()
    {
        Debug.Log("aaa");
        StartCoroutine(nameof(OpenDoorCoroutine));
    }
    
    private IEnumerator OpenDoorCoroutine()
    {
        float moveTime = 0f;

        while (moveTime < _duration)
        {
            Door.transform.Translate(_motionIncrement * Time.deltaTime);
            moveTime += Time.deltaTime;
            
            yield return null;
        }
    }
}
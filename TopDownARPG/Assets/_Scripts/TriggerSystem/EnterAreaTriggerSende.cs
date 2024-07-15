using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnterAreaTriggerSender : MonoBehaviour
{
    public List<TriggerReceiver> TriggerReceivers; 
    
    private bool isTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if(isTriggered) return;
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered the trigger area. Sending trigger to receivers.");
            foreach (var receiver in TriggerReceivers)
            {
                receiver.OnTriggerReceived();
            }
            isTriggered = true;
        }
    }
}
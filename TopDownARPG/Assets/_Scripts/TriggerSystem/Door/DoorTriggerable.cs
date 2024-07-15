using UnityEngine;

public class DoorTriggerable : MonoBehaviour, ITriggerable
{
    public GameObject door;

    public void OnTriggerReceived()
    {

        if (door != null)
        {
            door.SetActive(false);
            Debug.Log($"Door {door.name} has been triggered to open.");
        }
    }
}
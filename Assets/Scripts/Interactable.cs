using UnityEngine;
using UnityEngine.Events;


public enum InteractableType
{
    BatteryNormal,
    BatteryTrap,
    Bait
}

public class Interactable : MonoBehaviour
{
    static public float pickupRadius = 5f;

    public UnityEvent interactEvent;


    public void Interact()
    {
        interactEvent.Invoke();
    }
}

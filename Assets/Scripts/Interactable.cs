using UnityEngine;
using UnityEngine.Events;


public enum InteractableType
{
    Battery,
    Bait,
    Trap
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

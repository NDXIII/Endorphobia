using UnityEngine;
using UnityEngine.Events;


public enum InteractableType
{
    Battery,
    Bait
}

public abstract class Interactable : MonoBehaviour
{
    static public float pickupRadius = 5f;

    public UnityEvent interactEvent;


    public void Interact()
    {
        interactEvent.Invoke();
    }

    public abstract void Pickup();
    public new abstract InteractableType GetType();
}

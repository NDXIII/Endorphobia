using UnityEngine;
using UnityEngine.Events;


public enum InteractableType
{
    Battery,
    Bait
}

public class Interactable : MonoBehaviour
{
    public UnityEvent interactEvent;

    public void Interact()
    {
        interactEvent.Invoke();
    }
}

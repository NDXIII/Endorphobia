using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent interactEvent;

    public void Interact()
    {
        interactEvent.Invoke();
    }
}

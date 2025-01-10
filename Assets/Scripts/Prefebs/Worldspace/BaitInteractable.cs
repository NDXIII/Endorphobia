using UnityEngine;

public class BaitInteractable : MonoBehaviour
{
    public void Pickup()
    {
        GameManager.Instance.OnInteractablePickedUp(InteractableType.Bait, 1);
        Destroy(gameObject);
    }
}

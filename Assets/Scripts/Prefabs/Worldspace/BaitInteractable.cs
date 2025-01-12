using UnityEngine;

public class BaitInteractable : Interactable
{
    public override void Pickup()
    {
        GameManager.Instance.OnInteractablePickedUp(this);
        Destroy(gameObject);
    }

    public override InteractableType GetType()
    {
        return InteractableType.Bait;
    }
}

using UnityEngine;

public class BatteryInteractable : MonoBehaviour
{
    public float pickupChargeAmount = 0.25f;

    public void Pickup()
    {
        GameManager.Instance.OnInteractablePickedUp(InteractableType.Battery, pickupChargeAmount);
        Destroy(gameObject);
    }
}

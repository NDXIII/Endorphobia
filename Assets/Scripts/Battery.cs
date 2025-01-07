using UnityEngine;

public class Battery : MonoBehaviour
{
    public float pickupChargeAmount = 0.25f;

    public void Pickup()
    {
        BatteryManager.Instance.BatteryPickedUp(pickupChargeAmount);
        Destroy(gameObject);
    }
}

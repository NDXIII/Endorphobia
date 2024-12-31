using UnityEngine;

public class Battery : MonoBehaviour
{
    public float pickupChargeAmount = 0.25f;

    public void Pickup()
    {
        //Debug.Log("Battery picked up!");
        BatteryManager.Instance.BatteryPickedUp();
        BatteryManager.Instance.ChargeBattery(pickupChargeAmount);
        Destroy(gameObject);
    }
}

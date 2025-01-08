using UnityEngine;

public class Battery : MonoBehaviour
{
    public float pickupChargeAmount = 0.25f;

    public void Pickup()
    {
        GameManager.Instance.OnBatteryPickedUp(pickupChargeAmount);
        Destroy(gameObject);
    }
}

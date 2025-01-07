using TMPro;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public static BatteryManager Instance;

    public ObjectSpawner objectSpawner;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    public void BatteryPickedUp(float chargeAmount)
    {
        GameManager.Instance.OnBatteryPickedUp(chargeAmount);
        SpawnBatteries(1);
    }

    private void SpawnBatteries(int amount)
    {
        objectSpawner.SpawnAdditionalObjects(amount);
    }
}

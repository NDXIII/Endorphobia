using TMPro;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public static BatteryManager Instance;
    public float batteryCharge { get; private set; } = 1f;

    [Header("Battery Parameters")]
    public float batteryDepletionRate = 1f;

    [Header("References")]
    public ObjectSpawner objectSpawner;
    public GameObject flashlight;
    public TMP_Text txtCharge;


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

    // Update is called once per frame
    void Update()
    {
        batteryCharge -= batteryDepletionRate * Time.deltaTime * 0.01f;
        batteryCharge = Mathf.Clamp(batteryCharge, 0f, 1f);

        // Set flashlight active if battery charge is greater than 0.01f, otherwise deactivate it
        flashlight.SetActive(!(batteryCharge <= 0.01f));

        // Update battery text
        txtCharge.SetText((int)(batteryCharge * 100) + "%");
    }

    public void BatteryPickedUp()
    {
        SpawnBatteries(1);
    }

    public void ChargeBattery(float chargeAmount)
    {
        batteryCharge = Mathf.Clamp(batteryCharge + chargeAmount, 0f, 1f);
    }

    private void SpawnBatteries(int amount)
    {
        objectSpawner.SpawnAdditionalObjects(amount);
    }
}

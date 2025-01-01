using TMPro;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public static BatteryManager Instance;

    [Header("Battery Parameters")]
    public float batteryDepletionRate = 1f;

    [Header("References")]
    public ObjectSpawner objectSpawner;
    public GameObject flashlight;
    public TMP_Text batteryText;

    private float batteryCharge = 1f;

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

        // Update battery text in percentage with a fixed format of XXX%
        batteryText.text = (batteryCharge * 100).ToString("F0") + "%";
    }

    public void BatteryPickedUp()
    {
        SpawnBatteries(1);
    }

    public void ChargeBattery(float chargeAmount)
    {
        batteryCharge = Mathf.Clamp(batteryCharge + chargeAmount, 0f, 1f);
    }

    public float GetBatteryCharge()
    {
        return batteryCharge;
    }

    private void SpawnBatteries(int amount)
    {
        objectSpawner.SpawnAdditionalObjects(amount);
    }
}

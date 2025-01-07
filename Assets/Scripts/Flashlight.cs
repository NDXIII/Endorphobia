using TMPro;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Battery Parameters")]
    public float batteryCharge { get; private set; } = 1f;
    public float batteryDepletionRate = 1f;

    [Header("References")]
    public GameObject uiResource;

    private UiResource uiResourceExtracted;
    private float cutoffCharge = 0.01f;


    private void Start()
    {
        uiResourceExtracted = uiResource.GetComponent<UiResource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Only discharge if flashlight active
        if (uiResourceExtracted.active)
        {
            // Calculate new charge
            batteryCharge -= batteryDepletionRate * Time.deltaTime * 0.01f;
            batteryCharge = Mathf.Clamp(batteryCharge, 0f, 1f);

            // Are we under the cutoff charge
            if (batteryCharge <= cutoffCharge)
            {
                // Disable flashlight
                gameObject.SetActive(false);
                uiResourceExtracted.Toggle();
            }

            // Update battery text
            UpdateUi();
        }
    }

    private void UpdateUi()
    {
        uiResourceExtracted.SetDetailText((int)(batteryCharge * 100) + "%");
    }


    public void Toggle()
    {
        // Is the battery full enough?
        if (batteryCharge > cutoffCharge)
        {
            // Toggle flashlight
            gameObject.SetActive(!uiResourceExtracted.active);
            uiResourceExtracted.Toggle();
        }
    }

    public void ChargeBattery(float chargeAmount)
    {
        // Charge battery up again
        batteryCharge = Mathf.Clamp(batteryCharge + chargeAmount, 0f, 1f);
        UpdateUi();
    }
}

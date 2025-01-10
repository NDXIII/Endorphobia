using UnityEngine;

public class FlashlightTool : MonoBehaviour
{
    public float batteryCharge { get; private set; } = 1f;

    public float batteryDepletionRate = 1f;
    public GameObject uiToolObject;

    private Light lightSource;
    private AudioSource audioSource;
    private UITool uiToolClass;
    private float cutoffCharge = 0.01f;


    private void Start()
    {
        // Get components
        lightSource = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
        uiToolClass = uiToolObject.GetComponent<UITool>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Only discharge if flashlight active
        if (uiToolClass.selected)
        {
            // Calculate new charge
            batteryCharge -= batteryDepletionRate * Time.deltaTime * 0.01f;
            batteryCharge = Mathf.Clamp(batteryCharge, 0f, 1f);

            // Are we under the cutoff charge
            if (batteryCharge <= cutoffCharge)
            {
                // Disable flashlight
                lightSource.enabled = false;
                uiToolClass.Select(false);
            }

            // Update battery text
            UpdateUi();
        }
    }

    private void UpdateUi()
    {
        uiToolClass.SetDetailText((int)(batteryCharge * 100) + "%");
    }

    public void Toggle()
    {
        // Is the battery full enough?
        if (batteryCharge > cutoffCharge)
        {
            // Toggle flashlight
            lightSource.enabled = !lightSource.enabled;
            uiToolClass.Select(!uiToolClass.selected);

            // Play sound effect
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void ChargeBattery(float chargeAmount)
    {
        // Charge battery up again
        batteryCharge = Mathf.Clamp(batteryCharge + chargeAmount, 0f, 1f);
        UpdateUi();
    }
}

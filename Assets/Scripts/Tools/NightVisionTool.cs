using UnityEngine;
using UnityEngine.Rendering;

public class NightVisionTool : MonoBehaviour
{
    public float batteryCharge { get; private set; } = 1f;

    public float batteryDepletionRate = 1f;
    public Volume nightVisionVolume;
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

        // Set start state
        SwitchOn(uiToolClass.selected);
    }

    // Update is called once per frame
    private void Update()
    {
        // Only discharge if night vision active
        if (uiToolClass.selected)
        {
            // Calculate new charge
            batteryCharge -= batteryDepletionRate * Time.deltaTime * 0.01f;
            batteryCharge = Mathf.Clamp(batteryCharge, 0f, 1f);

            // Are we under the cutoff charge
            if (batteryCharge <= cutoffCharge)
            {
                // Disable night vision
                SwitchOn(false);
            }

            // Update battery text
            UpdateUi();
        }
    }

    private void UpdateUi()
    {
        // Update charge text
        uiToolClass.SetDetailText((int)(batteryCharge * 100) + "%");
    }

    public void SwitchOn(bool enable)
    {
        // Disable if battery dead
        if (batteryCharge <= cutoffCharge)
        {
            enable = false;
        }

        // Play sound effect if state changed
        if (uiToolClass.selected != enable)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        // Set new state
        nightVisionVolume.enabled = enable;
        lightSource.enabled = enable;
        uiToolClass.Select(enable);
    }

    public void Toggle()
    {
        // Toggle night vision
        SwitchOn(!uiToolClass.selected);
    }

    public void ChargeBattery(float chargeAmount)
    {
        // Charge battery up again
        batteryCharge = Mathf.Clamp(batteryCharge + chargeAmount, 0f, 1f);
        UpdateUi();
    }
}

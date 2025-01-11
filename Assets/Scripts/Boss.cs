using UnityEngine;

public class Boss : MonoBehaviour
{
    public float batteryTrapRadius = 3f;

    private AudioSource audioSource;


    private void Awake() {
        // Get components
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if there is a BatterInteractable object nearby
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, batteryTrapRadius);
        foreach (var hitCollider in hitColliders)
        {
            BatteryInteractable batteryInteractable = hitCollider.GetComponentInParent<BatteryInteractable>();
            if (batteryInteractable != null)
            {
                // If there is a BatteryInteractable object nearby, place a trap onto it
                if(!batteryInteractable.IsTrapped())
                    batteryInteractable.SetTrapped(true);
            }
        }

        // Play audio source only when the game is not paused
        if (Time.timeScale == 0f) {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }
}

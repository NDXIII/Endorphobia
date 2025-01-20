using Unity.Behavior;
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
        // Play audio source only when the game is not paused
        if (Time.timeScale == 0f && audioSource.isPlaying) {
            audioSource.Pause();
        }
        else if (Time.timeScale != 0f && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    private void FixedUpdate() {
        // Check if there is a BatterInteractable object nearby
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, batteryTrapRadius);
        foreach (var hitCollider in hitColliders)
        {
            BatteryInteractable batteryInteractable = hitCollider.GetComponentInParent<BatteryInteractable>();
            if (batteryInteractable != null)
            {
                // If there is a BatteryInteractable object nearby, place a trap onto it
                if(!batteryInteractable.trapped)
                    batteryInteractable.SetTrapped(true);
            }
        }
    }

    public void Reset()
    {
        GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "BaitCounter").ObjectValue = 0;
        transform.position = new Vector3(42, 0, -42);
    }
}

using UnityEngine;

public class Boss : MonoBehaviour
{
    public float batteryTrapRadius = 3f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    }
}
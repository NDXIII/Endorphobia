using UnityEngine;

public class PickupUI : MonoBehaviour
{
    private Canvas canvas;
    private Camera playerCamera;


    // Start is called before the first frame update
    private void Start()
    {
        // Get components
        canvas = GetComponent<Canvas>();
        playerCamera = GameManager.Instance.player.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, playerCamera.transform.position);

        // Look at player
        transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.transform.position);
        canvas.enabled = distanceToPlayer < Interactable.pickupRadius;
    }
}
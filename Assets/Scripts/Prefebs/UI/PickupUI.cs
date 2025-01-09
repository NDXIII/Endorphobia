using UnityEngine;

public class PickupUI : MonoBehaviour
{
    public float fadeOutDistance = 5.0f;

    private Canvas canvas;


    // Start is called before the first frame update
    private void Start()
    {
        // Get components
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get player transform
        Transform playerTransform = GameManager.Instance.player.transform;

        // Get distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Look at player
        transform.rotation = Quaternion.LookRotation(transform.position - playerTransform.position);
        canvas.enabled = distanceToPlayer < fadeOutDistance;
    }
}
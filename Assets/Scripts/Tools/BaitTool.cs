using UnityEngine;

public class BaitTool : MonoBehaviour
{
    public int amount { get; private set; } = 1;

    [Header("Game Objects")]
    public GameObject prefabObject;
    public GameObject uiToolObject;

    [Header("Throw Parameters")]
    public float throwForce = 25f;
    public float throwCooldown = 0.1f;

    private UITool uiToolClass;
    private GameObject currentObj;
    private float nextThrowTime = 0f;


    // Start is called before the first frame update
    private void Start()
    {
        // Get components
        uiToolClass = uiToolObject.GetComponent<UITool>();
        
        // Update the UI
        UpdateUi();
    }


    private void UpdateUi() {
        // Update text and seletion
        uiToolClass.SetDetailText(amount + "x");
        uiToolClass.Select(amount != 0);
    }


    public void Refill(int amount = 1)
    {
        // Calculate new amount
        this.amount += amount;

        // Update the UI
        UpdateUi();
    }

    public void Throw()
    {
        // Check if we have any baits
        if (amount <= 0)
        {
            return;
        }

        // Cooldown
        if (Time.time < nextThrowTime)
        {
            return;
        }
        nextThrowTime = Time.time + throwCooldown;


        // Instantiate bait prefab
        if (currentObj != null)
        {
            Destroy(currentObj);
        }
        currentObj = Instantiate(prefabObject, transform.position, transform.rotation);

        // Add force to bait
        Rigidbody rb = currentObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        // Update new amount
        amount--;

        // Update the UI
        UpdateUi();
    }
}
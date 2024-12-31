using UnityEngine;

public class Battery : MonoBehaviour
{
    public void Pickup()
    {
        Debug.Log("Battery picked up!");
        Destroy(gameObject);
    }
}

using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Stone hit: " + transform.position);
        Destroy(gameObject);
    }
}

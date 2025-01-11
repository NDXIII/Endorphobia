using Unity.Behavior;
using UnityEngine;

public class BatteryInteractable : MonoBehaviour
{
    public bool isTrapped { get; private set; } = false;
    public float pickupChargeAmount = 0.25f;


    public void SetTrapped(bool trapped)
    {
        isTrapped = trapped;

        /*if(isTrapped)
        {
            Debug.Log("Battery " + this.gameObject + " has been trapped!");
        }*/
    }

    public void Pickup()
    {
        if (isTrapped)
        {
            TrapEvent trapEvent = ScriptableObject.CreateInstance<TrapEvent>();
            GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "LastTrapLocation").ObjectValue = transform.position;
            GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "TrapEvent").ObjectValue = trapEvent;
            trapEvent.SendEventMessage();
        }

        // Notify game manager
        GameManager.Instance.OnInteractablePickedUp(isTrapped ? InteractableType.BatteryTrap : InteractableType.BatteryNormal, pickupChargeAmount);

        // Destroy this object
        Destroy(gameObject);
    }
}

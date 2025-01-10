using Unity.Behavior;
using UnityEngine;

public class BatteryInteractable : MonoBehaviour
{
    public float pickupChargeAmount = 0.25f;
    private bool isTrapped = false;

    public void SetTrapped(bool trapped)
    {
        isTrapped = trapped;

        if(isTrapped)
        {
            Debug.Log("Battery " + this.gameObject + " has been trapped!");
        }
    }

    public bool IsTrapped()
    {
        return isTrapped;
    }

    public void Pickup()
    {
        if (isTrapped)
        {
            Debug.Log("Battery is trapped!");
            TrapEvent trapEvent = ScriptableObject.CreateInstance<TrapEvent>();
            GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "LastTrapLocation").ObjectValue = transform.position;
            GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "TrapEvent").ObjectValue = trapEvent;
            trapEvent.SendEventMessage();
        }

        GameManager.Instance.OnInteractablePickedUp(InteractableType.Battery, pickupChargeAmount);
        Destroy(gameObject);
    }
}

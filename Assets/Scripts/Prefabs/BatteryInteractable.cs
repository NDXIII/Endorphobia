using Unity.Behavior;
using UnityEngine;

public class BatteryInteractable : Interactable
{
    public bool trapped { get; private set; } = false;

    public float pickupChargeAmount = 0.25f;


    public void SetTrapped(bool trapped)
    {
        this.trapped = trapped;

        /*if(isTrapped)
        {
            Debug.Log("Battery " + this.gameObject + " has been trapped!");
        }*/
    }

    public override void Pickup()
    {
        if (trapped)
        {
            TrapEvent trapEvent = ScriptableObject.CreateInstance<TrapEvent>();
            GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "LastTrapLocation").ObjectValue = transform.position;
            GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "TrapEvent").ObjectValue = trapEvent;
            trapEvent.SendEventMessage();
        }

        // Notify game manager
        GameManager.Instance.OnInteractablePickedUp(this);

        // Destroy this object
        Destroy(gameObject);
    }

    public override InteractableType GetType()
    {
        return InteractableType.Battery;
    }
}

using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/TrapEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "TrapEvent", message: "Trap triggered", category: "Events", id: "8f294c130c8beb5d898c7d7b389419c8")]
public partial class TrapEvent : EventChannelBase
{
    public delegate void TrapEventEventHandler();
    public event TrapEventEventHandler Event; 

    public void SendEventMessage()
    {
        Event?.Invoke();
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        Event?.Invoke();
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        TrapEventEventHandler del = () =>
        {
            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as TrapEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as TrapEventEventHandler;
    }
}


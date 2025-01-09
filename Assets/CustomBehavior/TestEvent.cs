using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/TestEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "TestEvent", message: "Test", category: "Events", id: "59d1bcef7ea22244a3d732f14e53ee65")]
public partial class TestEvent : EventChannelBase
{
    public delegate void TestEventEventHandler();
    public event TestEventEventHandler Event; 

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
        TestEventEventHandler del = () =>
        {
            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as TestEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as TestEventEventHandler;
    }
}


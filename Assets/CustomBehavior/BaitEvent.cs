using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/BaitEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "BaitEvent", message: "[Agent] has spotted [Bait]", category: "Events", id: "8de58ad9a3e97fde8e47613e15ef6ead")]
public partial class BaitEvent : EventChannelBase
{
    public delegate void BaitEventEventHandler(GameObject Agent, GameObject Bait);
    public event BaitEventEventHandler Event; 

    public void SendEventMessage(GameObject Agent, GameObject Bait)
    {
        Event?.Invoke(Agent, Bait);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> AgentBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Agent = AgentBlackboardVariable != null ? AgentBlackboardVariable.Value : default(GameObject);

        BlackboardVariable<GameObject> BaitBlackboardVariable = messageData[1] as BlackboardVariable<GameObject>;
        var Bait = BaitBlackboardVariable != null ? BaitBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Agent, Bait);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        BaitEventEventHandler del = (Agent, Bait) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Agent;

            BlackboardVariable<GameObject> var1 = vars[1] as BlackboardVariable<GameObject>;
            if(var1 != null)
                var1.Value = Bait;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as BaitEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as BaitEventEventHandler;
    }
}


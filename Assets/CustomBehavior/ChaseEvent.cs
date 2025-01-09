using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/ChaseEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "ChaseEvent", message: "[Agent] starts chasing [Player]", category: "Events", id: "6af4303bde979a03ae13352ca7376a5f")]
public partial class ChaseEvent : EventChannelBase
{
    public delegate void ChaseEventEventHandler(GameObject Agent, GameObject Player);
    public event ChaseEventEventHandler Event; 

    public void SendEventMessage(GameObject Agent, GameObject Player)
    {
        Event?.Invoke(Agent, Player);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> AgentBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Agent = AgentBlackboardVariable != null ? AgentBlackboardVariable.Value : default(GameObject);

        BlackboardVariable<GameObject> PlayerBlackboardVariable = messageData[1] as BlackboardVariable<GameObject>;
        var Player = PlayerBlackboardVariable != null ? PlayerBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Agent, Player);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        ChaseEventEventHandler del = (Agent, Player) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Agent;

            BlackboardVariable<GameObject> var1 = vars[1] as BlackboardVariable<GameObject>;
            if(var1 != null)
                var1.Value = Player;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as ChaseEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as ChaseEventEventHandler;
    }
}


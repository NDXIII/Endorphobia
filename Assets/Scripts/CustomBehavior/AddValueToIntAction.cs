using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnityEngine.Rendering.DebugUI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Add Value to Int", story: "Add [number] to [variable]", category: "Action", id: "bbf034e08fb7a354f99d774b11e1588f")]
public partial class AddValueToIntAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Number;
    [SerializeReference] public BlackboardVariable<int> Variable;

    protected override Status OnStart()
    {
        if (Variable == null || Number == null)
        {
            return Status.Failure;
        }
        Variable.ObjectValue = (int)Variable.ObjectValue + (int)Number.ObjectValue;
        return Status.Success;
    }

    /*
    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
    */
}


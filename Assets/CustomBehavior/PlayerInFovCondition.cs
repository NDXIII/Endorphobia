using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "PlayerInFOV", story: "[Player] is in [FOV] degree FOV and [Range] range of [Agent]", category: "Conditions", id: "6a200b8e9904935dea18c7eda2dd0959")]
public partial class PlayerInFovCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> FOV;
    [SerializeReference] public BlackboardVariable<float> Range;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    public override bool IsTrue()
    {
        return CanSeePlayer();
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }

    private bool CanSeePlayer()
    {
        Transform player = Player.Value.transform;
        Transform agent = Agent.Value.transform;
        float fovAngle = FOV.Value;
        float viewDistance = Range.Value;

        // Calculate direction to the player
        Vector3 directionToPlayer = (player.position - agent.position).normalized;

        // Check if the player is within the FOV
        float angleToPlayer = Vector3.Angle(agent.forward, directionToPlayer);
        if (angleToPlayer > fovAngle / 2f)
        {
            Debug.Log("Player is outside the FOV");
            return false; // Player is outside the FOV
        }

        // Check if the player is within the view distance
        float distanceToPlayer = Vector3.Distance(agent.position, player.position);
        if (distanceToPlayer > viewDistance)
        {
            Debug.Log("Player is too far away");
            return false; // Player is too far away
        }

        // Perform a raycast to check for obstacles
        if (Physics.Raycast(agent.position, directionToPlayer, out RaycastHit hit, viewDistance, LayerMask.GetMask("Wall")))
        {
            if (hit.transform != player)
            {
                Debug.Log("Player is obstructed by " + hit.transform.name);
                return false; // Something is obstructing the view
            }
        }

        // Player is within FOV, within range, and not obstructed
        Debug.Log("Player is within FOV, within range, and not obstructed");
        return true;
    }
}

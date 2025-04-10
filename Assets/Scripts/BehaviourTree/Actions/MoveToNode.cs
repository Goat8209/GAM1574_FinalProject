using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNode : ActionNode
{
    public string destinationKey;
    public float tolerance = 1.0f;
    private bool noDestination = false;

    protected override void OnStart()
    {
        if (blackboard.ContainsKey(destinationKey, Blackboard.ValueType.Vector3))
        {
            noDestination = false;
            Vector3 destination = blackboard.GetValue<Vector3>(destinationKey);
            context.agent.destination = destination;
        }
        else
        {
            noDestination = true;
            Debug.LogAssertion("[MoveToNode] Destination key: " + destinationKey + " is not in the blackboard");
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (noDestination)
        {
            return State.Failure;
        }

        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Running;
    }
}

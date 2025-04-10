using System;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : DecoratorNode
{
    public string conditionKey;
    private bool conditionValue;
    private bool noCondition;

    protected override void OnStart()
    {
        if (blackboard.ContainsKey(conditionKey, Blackboard.ValueType.Bool) == false)
        {
            noCondition = true;
            Debug.LogAssertion("[ConditionalNode] Conditional key: " + conditionKey + " is not in the blackboard");
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (blackboard.ContainsKey(conditionKey, Blackboard.ValueType.Bool))
        {
            conditionValue = blackboard.GetValue<bool>(conditionKey);
            noCondition = false;
        }

        if (noCondition)
        {
            return State.Failure;
        }

        if (conditionValue)
        {
            return child.Update();
        }

        return State.Failure;
    }
}

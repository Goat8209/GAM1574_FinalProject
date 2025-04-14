using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNodeString : Node
{
    [HideInInspector] public Node childTrue;
    [HideInInspector] public Node childFalse;

    public string conditionKey;
    public string condition;
    private bool conditionValue;
    private bool noCondition;

    public override Node Clone()
    {
        ConditionNodeString node = Instantiate(this);
        node.childTrue = childTrue.Clone();
        node.childFalse = childFalse.Clone();
        return node;
    }

    protected override void OnStart()
    {
        if (blackboard.ContainsKey(conditionKey, Blackboard.ValueType.String) == false)
        {
            noCondition = true;
            Debug.LogAssertion("[ConditionNode] Conditional key: " + conditionKey + " is not in the blackboard");
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (blackboard.ContainsKey(conditionKey, Blackboard.ValueType.String))
        {
            if (blackboard.GetValue<string>(conditionKey) == condition)
            {
                conditionValue = true;
            }
            else
            {
                conditionValue = false;
            }
            noCondition = false;
        }

        if (noCondition)
        {
            return State.Failure;
        }

        //Debug.Log("[ConditionNode] [" + conditionKey + "] = " + conditionValue);

        if (conditionValue)
        {
            return childTrue.Update();
        }

        return childFalse.Update();
    }
}

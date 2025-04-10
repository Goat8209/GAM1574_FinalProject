using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : Node
{
    [HideInInspector] public Node childTrue;
    [HideInInspector] public Node childFalse;

    public string conditionKey;
    private bool conditionValue;
    private bool noCondition;

    public override Node Clone()
    {
        ConditionNode node = Instantiate(this);
        node.childTrue = childTrue.Clone();
        node.childFalse = childFalse.Clone();
        return node;
    }

    protected override void OnStart()
    {
        if (blackboard.ContainsKey(conditionKey, Blackboard.ValueType.Bool) == false)
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
        if (blackboard.ContainsKey(conditionKey, Blackboard.ValueType.Bool))
        {
            conditionValue = blackboard.GetValue<bool>(conditionKey);
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

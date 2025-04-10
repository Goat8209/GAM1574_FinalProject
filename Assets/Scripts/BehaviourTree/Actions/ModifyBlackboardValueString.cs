using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : ActionNode
{
    public string entryKey;
    public string newValue;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (blackboard.ContainsKey(entryKey, Blackboard.ValueType.String) == false)
        {
            return State.Failure;
        }

        blackboard.SetValue<string>(entryKey, newValue);
        return State.Success;
    }
}

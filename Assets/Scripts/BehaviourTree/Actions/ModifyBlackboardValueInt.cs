using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyBlackboardValueInt : ActionNode
{
    public string entryKey;
    public int newValue;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (blackboard.ContainsKey(entryKey, Blackboard.ValueType.Int) == false)
        {
            return State.Failure;
        }

        blackboard.SetValue<int>(entryKey, newValue);
        return State.Success;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyBlackboardValueBool : ActionNode
{
    public string entryKey;
    public bool newValue;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (blackboard.ContainsKey(entryKey, Blackboard.ValueType.Bool) == false)
        {
            return State.Failure;
        }

        blackboard.SetValue<bool>(entryKey, newValue);
        return State.Success;
    }
}

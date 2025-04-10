using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyBlackboardValueFloat : ActionNode
{
    public string entryKey;
    public float newValue;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (blackboard.ContainsKey(entryKey, Blackboard.ValueType.Float) == false)
        {
            return State.Failure;
        }

        blackboard.SetValue<float>(entryKey, newValue);
        return State.Success;
    }
}

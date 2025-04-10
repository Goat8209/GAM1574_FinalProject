using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyBlackboardValueVector3 : ActionNode
{
    public string entryKey;
    public Vector3 newValue;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (blackboard.ContainsKey(entryKey, Blackboard.ValueType.Vector3) == false)
        {
            return State.Failure;
        }

        blackboard.SetValue<Vector3>(entryKey, newValue);
        return State.Success;
    }
}

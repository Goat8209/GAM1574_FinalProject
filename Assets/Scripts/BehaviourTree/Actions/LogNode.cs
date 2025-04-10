using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogNode : ActionNode
{
    public string message;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        Debug.Log("OnUpdate(" + message + ")");
        return State.Success;
    }
}

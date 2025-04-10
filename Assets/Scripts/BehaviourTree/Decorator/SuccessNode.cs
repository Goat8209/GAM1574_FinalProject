using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessNode : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        State state = child.Update();
        if (state == State.Success || state == State.Failure)
        {
            return State.Success;
        }

        return state;
    }
}

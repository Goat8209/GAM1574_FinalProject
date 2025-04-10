using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntilSuccessNode : DecoratorNode
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
        State returnState = state == State.Success ? State.Success : State.Running;
        return returnState;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UntilFailureNode : DecoratorNode
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
        State returnState = state == State.Failure ? State.Success : State.Running;
        return returnState;
    }
}

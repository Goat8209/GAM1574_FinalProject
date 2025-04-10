using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryNode : DecoratorNode
{
    public uint maxAttempts = 1;
    private uint attemptCounter = 0;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        State state = child.Update();
        if (state == State.Failure)
        {
            attemptCounter++;
            State returnState = attemptCounter > maxAttempts ? State.Failure : State.Running;
            return returnState;
        }

        return state;
    }
}

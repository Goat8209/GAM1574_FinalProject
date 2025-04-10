using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterNode : DecoratorNode
{
    public int repeatCount = -1; // -1 loops infinitely
    private int repeatCounter = 0;

    protected override void OnStart()
    {
        repeatCounter = repeatCount;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        State nodeState = child.Update();

        // A repeatCount of -1 means infinite repeats
        if (repeatCount == -1)
            return State.Running;

        // Did the child node fail?
        if (nodeState == State.Failure)
            return nodeState;

        // Decrement the repeatCounter
        if (nodeState == State.Success)
            repeatCounter--;

        // Return the node state
        State returnState = repeatCounter < 0 ? State.Success : State.Running;
        return returnState;
    }
}

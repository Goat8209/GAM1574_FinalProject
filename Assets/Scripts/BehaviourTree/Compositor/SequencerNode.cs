using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositorNode
{
    private int currentChildIndex;

    protected override void OnStart()
    {
        currentChildIndex = 0;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        for (int i = currentChildIndex; i < children.Count; ++i)
        {
            currentChildIndex = i;
            Node child = children[currentChildIndex];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    continue;
            }
        }

        return State.Success;
    }
}

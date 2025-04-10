using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectorNode : CompositorNode
{
    protected int childIndex;

    protected override void OnStart()
    {
        childIndex = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        for (int i = childIndex; i < children.Count; ++i)
        {
            childIndex = i;
            Node child = children[childIndex];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    continue;
            }
        }

        return State.Failure;
    }
}

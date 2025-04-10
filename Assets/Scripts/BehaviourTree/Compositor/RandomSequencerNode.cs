using System;
using System.Collections.Generic;
using System.Linq;

public class RandomSequenceNode : CompositorNode
{
    protected int childIndex;
    private List<Node> shuffledChildren;

    protected override void OnStart()
    {
        childIndex = 0;
        shuffledChildren = children.Randomize().ToList<Node>();
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        for (int i = childIndex; i < shuffledChildren.Count; ++i)
        {
            childIndex = i;
            Node child = shuffledChildren[childIndex];

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

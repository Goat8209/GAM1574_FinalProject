using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSelectorNode : CompositorNode
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
                case State.Success:
                    return State.Success;
                case State.Failure:
                    continue;
            }
        }

        return State.Failure;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositorNode : Node
{
    [HideInInspector] public List<Node> children = new List<Node>();

    public override Node Clone()
    {
        CompositorNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());
        return node;
    }
}

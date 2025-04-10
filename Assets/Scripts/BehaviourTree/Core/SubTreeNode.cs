using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTreeNode : Node
{
    public BehaviourTree tree;

    protected override void OnStart()
    {
        if (tree != null)
        {
            tree = tree.Clone();
        }
        else
        {
            Debug.Log("[SubTreeNode] Tree is null");
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return tree.Update();
    }
}

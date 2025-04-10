using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "new BehaviourTree", menuName = "Behaviour Tree/BehaviourTree")]
public class BehaviourTree : ScriptableObject
{
    public Blackboard blackboard;
    public Node rootNode;
    public Node.State treeState = Node.State.Running;
    public List<Node> nodes = new List<Node>();


    public Node.State Update()
    {
        if (rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }
        return treeState;
    }
    
    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        return node;
    }

    public void DeleteNode(Node node)
    {
        RootNode root = node as RootNode;
        if(root)
        {
            rootNode = null;
        }

        nodes.Remove(node);

        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if(decorator)
        {
            decorator.child = child;
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            root.child = child;
        }

        CompositorNode compositor = parent as CompositorNode;
        if (compositor)
        {
            compositor.children.Add(child);
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = null;
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            root.child = null;
        }

        CompositorNode compositor = parent as CompositorNode;
        if (compositor)
        {
            compositor.children.Remove(child);
        }
    }

    public static List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        RootNode root = parent as RootNode;
        if (root && root.child != null)
        {
            children.Add(root.child);
        }

        CompositorNode compositor = parent as CompositorNode;
        if (compositor)
        {
            return compositor.children;
        }

        ConditionNode conditionNode = parent as ConditionNode;
        if (conditionNode && conditionNode.childTrue != null)
        {
            children.Add(conditionNode.childTrue);
        }

        if (conditionNode && conditionNode.childFalse != null)
        {
            children.Add(conditionNode.childFalse);
        }

        return children;
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        return tree;
    }

    public static void Traverse(Node node, System.Action<Node> visitor)
    {
        if (node)
        {
            visitor.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visitor));
        }
    }

    public void Initialize(Context context)
    {
        treeState = Node.State.Running;

        Traverse(rootNode, node => 
        {
            node.state = Node.State.Running;
            node.context = context;
            node.blackboard = blackboard;
        });
    }
}

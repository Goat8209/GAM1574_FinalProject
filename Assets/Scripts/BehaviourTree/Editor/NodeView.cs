using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using Unity.VisualScripting.InputSystem;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public Node node;
    public Port input;
    public Port output;
    public Port outputConditionFalse;


    public NodeView(Node node) : base("Assets/UIBuilder/NodeView.uxml")
    {
        this.node = node;
        this.node.name = node.GetType().Name;
        this.title = node.name.Replace("(Clone)", "").Replace("Node", "");
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupClasses();
        SetupDataBinding();
    }

    private void SetupDataBinding()
    {
        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "description";
        descriptionLabel.Bind(new SerializedObject(node));
    }

    private void SetupClasses()
    {
        if (node is ActionNode)
        {
            AddToClassList("action");
        }
        else if (node is CompositorNode)
        {
            AddToClassList("compositor");
        }
        else if (node is DecoratorNode)
        {
            AddToClassList("decorator");
        }
        else if (node is ConditionNode)
        {
            AddToClassList("condition");
        }
        else if (node is RootNode)
        {
            AddToClassList("root");
        }
    }

    private void CreateInputPorts()
    {
        if (node is ActionNode)
        {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        }
        else if (node is CompositorNode)
        {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        }
        else if (node is DecoratorNode)
        {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        }
        else if (node is ConditionNode)
        {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        }
        else if (node is RootNode)
        {

        }

        if (input != null)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is ConditionNode)
        {
            output = new NodePort(Direction.Output, Port.Capacity.Single, Orientation.Vertical);
            output.portName = "True";
            output.portColor = Color.green;
            output.style.flexDirection = FlexDirection.ColumnReverse;

            outputContainer.Add(output);

            outputConditionFalse = new NodePort(Direction.Output, Port.Capacity.Single, Orientation.Vertical);
            outputConditionFalse.portName = "False";
            outputConditionFalse.portColor = Color.red;
            outputConditionFalse.style.flexDirection = FlexDirection.ColumnReverse;

            outputContainer.style.flexDirection = FlexDirection.Row;
            outputContainer.Add(outputConditionFalse);

        }
        else
        {
            if (node is ActionNode)
            {

            }
            else if (node is CompositorNode)
            {
                output = new NodePort(Direction.Output, Port.Capacity.Multi);
            }
            else if (node is DecoratorNode)
            {
                output = new NodePort(Direction.Output, Port.Capacity.Single);
            }
            else if (node is RootNode)
            {
                output = new NodePort(Direction.Output, Port.Capacity.Single);
            }

            if (output != null)
            {
                output.portName = "";
                output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);
            }
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(node, "Behaviour Tree (Set Position)");
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
        EditorUtility.SetDirty(node);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    public void SortChildren()
    {
        if (node is CompositorNode compositor)
        {
            compositor.children.Sort(SortByHorizontalPosition);
        }
    }

    private int SortByHorizontalPosition(Node left, Node right)
    {
        return left.position.x < right.position.x ? -1 : 1;
    }

    public void UpdateState()
    { 
        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");

        if (Application.isPlaying)
        {
            switch (node.state)
            {
                case Node.State.Running:
                    if (node.isStarted)
                    {
                        AddToClassList("running");
                    }
                    break;
                case Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Node.State.Success:
                    AddToClassList("success");
                    break;
            }
        }
    }
}
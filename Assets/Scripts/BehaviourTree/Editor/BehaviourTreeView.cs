using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

public class BehaviourTreeView : GraphView
{

    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
    BehaviourTree tree;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new DoubleClickSelection());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UIBuilder/BehaviourTreeEditorStyle.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    public NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());
        graphViewChanged += OnGraphViewChanged;

        if (tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        // Creates node view
        tree.nodes.ForEach(n => CreateNodeView(n));

        // Create edges
        tree.nodes.ForEach(n =>
        {
            ConditionNode conditionNode = n as ConditionNode;
            if (conditionNode)
            {
                NodeView parentView = FindNodeView(conditionNode);

                if (conditionNode.childTrue != null)
                {
                    NodeView childTrueView = FindNodeView(conditionNode.childTrue);
                    Edge edgeTrue = parentView.output.ConnectTo(childTrueView.input);
                    AddElement(edgeTrue);
                }

                if (conditionNode.childFalse != null)
                {
                    NodeView childFalseView = FindNodeView(conditionNode.childFalse);
                    Edge edgeFalse = parentView.outputConditionFalse.ConnectTo(childFalseView.input);
                    AddElement(edgeFalse);
                }
            }
            else
            {
                var children = BehaviourTree.GetChildren(n);
                children.ForEach(c =>
                {
                    NodeView parentView = FindNodeView(n);
                    NodeView childView = FindNodeView(c);

                    Edge edge = parentView.output.ConnectTo(childView.input);
                    AddElement(edge);
                });
            }
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;

                    ConditionNode conditionNode = parentView.node as ConditionNode;
                    if (conditionNode)
                    {
                        if (edge.output.portName == "True")
                        {
                            conditionNode.childTrue = null;
                        }
                        else if (edge.output.portName == "False")
                        {
                            conditionNode.childFalse = null;
                        }
                    }
                    else
                    {
                        tree.RemoveChild(parentView.node, childView.node);
                    }
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;

                ConditionNode conditionNode = parentView.node as ConditionNode;
                if (conditionNode)
                {
                    if(edge.output.portName == "True")
                    {
                        conditionNode.childTrue = childView.node;
                    }
                    else if (edge.output.portName == "False")
                    {
                        conditionNode.childFalse = childView.node;
                    }
                }
                else
                {
                    tree.AddChild(parentView.node, childView.node);
                }                
            });
        }

        nodes.ForEach((n) =>
        {
            NodeView view = n as NodeView;
            view.SortChildren();
        });

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

        // Actions nodes
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Action]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        // Compositor nodes
        {
            var types = TypeCache.GetTypesDerivedFrom<CompositorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Compositor]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        // Decorator nodes
        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Decorator]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        // Condition node
        {
            Type type = typeof(ConditionNode);
            {
                evt.menu.AppendAction($"[Condition]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }
    }

    void CreateNode(System.Type type, Vector2 position)
    {
        Node node = tree.CreateNode(type);
        node.position = position;
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeStates()
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }
}

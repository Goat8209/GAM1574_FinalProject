using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

public class BehaviourTreeEditor : EditorWindow
{
    BehaviourTreeView treeView;
    BehaviourTree tree;
    InspectorView inspectorView;
    IMGUIContainer blackboardView;
    Editor blackboardEditor;

    [MenuItem("Window/Behaviour Tree Editor")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("Behaviour Tree Editor");
        wnd.minSize = new Vector2(800, 600);
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UIBuilder/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UIBuilder/BehaviourTreeEditorStyle.uss");
        root.styleSheets.Add(styleSheet);

        // Main treeview
        treeView = root.Q<BehaviourTreeView>();
        treeView.OnNodeSelected = OnNodeSelectionChanged;

        // Inspector View
        inspectorView = root.Q<InspectorView>();

        // Blackboard view
        blackboardView = root.Q<IMGUIContainer>();

        if (tree == null)
        {
            OnSelectionChange();
        }
        else
        {
            SelectTree(tree);
        }
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }

    private void OnSelectionChange()
    {
        EditorApplication.delayCall += () =>
        {
            BehaviourTree tree = Selection.activeObject as BehaviourTree;
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                if (treeView != null)
                {
                    SelectTree(tree);
                }
            }
        };
    }

    void SelectTree(BehaviourTree newTree)
    {
        if (treeView == null)
        {
            return;
        }

        if (!newTree)
        {
            return;
        }

        this.tree = newTree;

        treeView.PopulateView(tree);

        if (tree != null)
        {
            if (tree.blackboard != null)
            {
                blackboardEditor = UnityEditor.Editor.CreateEditor(tree.blackboard);
                blackboardView.onGUIHandler = blackboardEditor.OnInspectorGUI;
            }
            else
            {
                blackboardEditor = null;
                blackboardView.onGUIHandler = null;
            }
        }

        EditorApplication.delayCall += () =>
        {
            treeView.FrameAll();
        };
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        treeView?.UpdateNodeStates();
    }
}

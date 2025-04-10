//using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    private Context context;

    // Start is called before the first frame update
    void Start()
    {
        context = Context.CreateFromGameObject(gameObject);

        tree = tree.Clone();
        tree.Initialize(context);
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}

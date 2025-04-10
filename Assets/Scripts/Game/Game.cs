using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Game : MonoBehaviour
{
    private static Game sInstance;

    public static Game Instance
    {
        get { return sInstance; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup the static instance of the Game class
        if (sInstance != null && sInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sInstance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

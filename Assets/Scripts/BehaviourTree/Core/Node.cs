using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running, 
        Failure, 
        Success
    }

    public State state = State.Running;
    [HideInInspector] public bool isStarted = false;
    [HideInInspector] public Context context;
    [HideInInspector] public Blackboard blackboard;

    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [TextArea] public string description;

    public State Update()
    {
        if(isStarted == false)
        {
            OnStart();
            isStarted = true;
        }

        state = OnUpdate();

        if(state == State.Failure || state == State.Success)
        {
            OnStop();
            isStarted = false;
        }

        return state;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    public void Reset()
    {
        state = State.Running;
    }

    public virtual void OnDrawGizmos() { }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}

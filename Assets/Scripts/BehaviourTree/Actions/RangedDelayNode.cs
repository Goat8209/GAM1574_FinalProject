using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDelayNode : ActionNode
{
    public float minDuration;
    public float maxDuration;
    private float duration;

    protected override void OnStart()
    {
        duration = Random.Range(minDuration, maxDuration);
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        duration -= Time.deltaTime;
        if (duration <= 0.0f)
        {
            duration = 0.0f;
            return State.Success;
        }


        return State.Running;
    }
}

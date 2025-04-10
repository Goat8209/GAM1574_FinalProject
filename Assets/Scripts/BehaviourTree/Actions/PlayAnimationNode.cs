using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationNode : ActionNode
{
    public string animationKey;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        context.animal.PlayAnimation(animationKey);
        return State.Success;
    }
}

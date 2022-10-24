using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;

public class RandomPosition : ActionNode
{
    public Vector2 min = Vector2.one * -10;
    public Vector2 max = Vector2.one * 10;

    protected override void OnStart() {
        context.aiAgent.weaponIK.enabled = false;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.aiAgent.aiHealth.GetIsDead())
        {
            return State.Failure;
        }

        if (context.aiAgent.sensor.Scan())
        {
            return State.Failure;
        }

        blackboard.moveToPosition.x = Random.Range(context.aiAgent.transform.position.x + min.x, context.aiAgent.transform.position.x + max.x);
        blackboard.moveToPosition.z = Random.Range(context.aiAgent.transform.position.z + min.y, context.aiAgent.transform.position.z + max.y);
        return State.Success;
    }
}

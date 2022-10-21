using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;
using UnityEngine.InputSystem;

public class ScanForTargets : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.aiAgent.sensor.Objects.Count > 0)
        {
            blackboard.target = context.aiAgent.sensor.Objects[0];
            blackboard.moveToPosition = new Vector3(blackboard.target.transform.position.x, 0.0f, blackboard.target.transform.position.z);
            return State.Success;
        }

        return State.Failure;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;

public class MoveToPosition : ActionNode
{
    public float speed = 5.0f;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart() {
        context.aiAgent.weaponIK.enabled = false;
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
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

        if (context.agent.pathPending) {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, context.agent.velocity.magnitude);
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance) {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, 0.0f);
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, 0.0f);

            return State.Failure;
        }


        context.animator.SetFloat(context.aiAgent.aiData.speedParam, context.agent.velocity.magnitude);
        return State.Running;
    }
}

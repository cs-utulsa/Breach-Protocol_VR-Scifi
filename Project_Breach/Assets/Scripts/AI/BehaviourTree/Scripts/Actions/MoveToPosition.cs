using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;

public class MoveToPosition : ActionNode
{
    protected override void OnStart() {
        context.aiAgent.weaponIK.enabled = false;
        context.agent.stoppingDistance = context.aiAgent.aiData.standardStoppingDistance;
        context.agent.speed = context.aiAgent.aiData.walkspeed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = true;
        context.agent.acceleration = context.aiAgent.aiData.acceleration;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        // Check if the AI should transition to the death action.
        if (context.aiAgent.aiHealth.GetIsDead())
        {
            return State.Failure;
        }

        // If it is alive, then scan for enemies.
        // If an enemy is detected in the scan, stop moving to the position and return to the root node where it will go to the attacking branch.
        if (context.aiAgent.sensor.Scan())
        {
            return State.Failure;
        }

        // If the AI is still figuring out how to get to the location, return running.
        if (context.agent.pathPending) {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, context.agent.velocity.magnitude);
            return State.Running;
        }

        // If the AI is close enough to the position it wants to go to, return success.
        if (context.agent.remainingDistance < context.aiAgent.aiData.moveTolerance) {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, 0.0f);
            return State.Success;
        }

        // If the AI can't get to the position anymore, return failure.
        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, 0.0f);

            return State.Failure;
        }

        // All if statements failed, continue to move  to position and update animator.
        context.animator.SetFloat(context.aiAgent.aiData.speedParam, context.agent.velocity.magnitude);
        return State.Running;
    }
}

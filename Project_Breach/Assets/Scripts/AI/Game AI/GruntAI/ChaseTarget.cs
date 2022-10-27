using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;

public class ChaseTarget : ActionNode
{

    protected override void OnStart()
    {
        // Reinitialize variables.
        context.aiAgent.weaponIK.enabled = false;
        context.agent.stoppingDistance = context.aiAgent.aiData.minAttackStoppingDistance;
        context.agent.speed = context.aiAgent.aiData.chaseSpeed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = true;
        context.agent.acceleration = context.aiAgent.aiData.acceleration;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // Check if the AI should transition to the death action.
        if (context.aiAgent.aiHealth.GetIsDead())
        {
            return State.Failure;
        }

        // If an enemy is no longer detected while chasing, return failure.
        if (!context.aiAgent.sensor.Scan())
        {
            return State.Failure;
        }
        else // If the enemy is detected, chase the first enemy detected.
        {
            // Set the target in the blackboard.
            blackboard.target = context.aiAgent.sensor.objects[0];

            // Set the target transform for the weapon inverse kinematics component. (This tells the AI what to aim at when it begins to shoot.)
            context.aiAgent.weaponIK.targetTransform = blackboard.target.transform;

            // Move the AI towards the target.
            blackboard.moveToPosition = new Vector3(blackboard.target.transform.position.x, 0.0f, blackboard.target.transform.position.z);
            context.agent.destination = blackboard.moveToPosition;
        }


        // If the AI is within the attack range, transition from chase to attack.
        if (Mathf.Abs(Vector3.Distance(context.agent.transform.position, blackboard.target.transform.position)) < context.aiAgent.aiData.attackRange)
        {
            context.animator.SetBool(context.aiAgent.aiData.attackParam, true);
            return State.Success;
        }

        // If the AI is still figuring out how to get to the location, return running.
        if (context.agent.pathPending)
        {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, context.agent.velocity.magnitude);
            return State.Running;
        }
        // If the AI is close enough to the position it wants to go to, return success.
        if (context.agent.remainingDistance < context.aiAgent.aiData.moveTolerance)
        {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, 0.0f);
            return State.Success;
        }

        // If the AI can't get to the position anymore, return failure.
        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            context.animator.SetFloat(context.aiAgent.aiData.speedParam, 0.0f);

            return State.Failure;
        }

        // All if statements failed, continue to move to position and update animator.
        context.animator.SetFloat(context.aiAgent.aiData.speedParam, context.agent.velocity.magnitude);
        return State.Running;
    }
}

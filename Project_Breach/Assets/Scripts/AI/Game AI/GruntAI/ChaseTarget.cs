using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;

public class ChaseTarget : ActionNode
{
    public float speed = 5.0f;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart()
    {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.aiAgent.sensor.target != null)
        {
            blackboard.target = context.aiAgent.sensor.target;
            return State.Failure;
        }

        if (context.agent.pathPending)
        {
            context.animator.SetFloat("Speed", context.agent.velocity.magnitude);
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            context.animator.SetFloat("Speed", 0.0f);
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            context.animator.SetFloat("Speed", 0.0f);

            return State.Failure;
        }

        blackboard.moveToPosition = new Vector3(blackboard.target.transform.position.x, 0.0f, blackboard.target.transform.position.z);
        context.animator.SetFloat("Speed", context.agent.velocity.magnitude);
        return State.Running;
    }
}

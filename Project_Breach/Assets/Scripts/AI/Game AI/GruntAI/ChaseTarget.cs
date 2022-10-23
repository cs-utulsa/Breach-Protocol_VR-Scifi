using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;
using UnityEditor.Rendering.LookDev;

public class ChaseTarget : ActionNode
{
    public float speed = 5.0f;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    public float attackRange = 10.0f;

    protected override void OnStart()
    {
        context.aiAgent.weaponIK.enabled = false;
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
        if (context.aiAgent.aiHealth.GetIsDead())
        {
            return State.Failure;
        }

        if (!context.aiAgent.sensor.Scan())
        {
            return State.Failure;
        } 
        else
        {
            blackboard.target = context.aiAgent.sensor.Objects[0];
            context.aiAgent.weaponIK.targetTransform = blackboard.target.transform;
            blackboard.moveToPosition = new Vector3(blackboard.target.transform.position.x, 0.0f, blackboard.target.transform.position.z);
            context.agent.destination = blackboard.moveToPosition;
        }

        if (Mathf.Abs(Vector3.Distance(context.agent.transform.position, blackboard.target.transform.position)) < attackRange)
        {
            context.animator.SetBool("Attack", true);
            return State.Success;
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

        Debug.Log("I am chasing the target.");
        context.animator.SetFloat("Speed", context.agent.velocity.magnitude);
        return State.Running;
    }
}

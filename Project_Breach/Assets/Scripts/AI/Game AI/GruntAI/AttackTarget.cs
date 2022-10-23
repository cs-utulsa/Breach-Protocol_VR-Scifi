using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;

public class AttackTarget : ActionNode
{
    public float speed = 1.0f;
    public float stoppingDistance = .1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    public float attackRange = 10.0f;
    public float timer = 0.0f;

    protected override void OnStart()
    {
        context.aiAgent.weaponIK.enabled = true;
        context.aiAgent.weaponIK.targetTransform = blackboard.target.transform;
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        context.agent.destination = blackboard.moveToPosition;
        context.animator.SetBool("Attack", true);
    }

    protected override void OnStop()
    {
        context.animator.SetBool("Attack", false);
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
        context.aiAgent.weaponIK.targetTransform = blackboard.target.transform;
        blackboard.moveToPosition = new Vector3(blackboard.target.transform.position.x, 0.0f, blackboard.target.transform.position.z);
        context.agent.destination = blackboard.moveToPosition;

        

        if (context.aiAgent.weapon.GetAmmo() > 0 && timer <= 0)
        {
            context.aiAgent.weapon.TriggerPulled();
            timer = Random.Range(context.aiAgent.aiData.minShootTimer, context.aiAgent.aiData.maxShootTimer);
        }
        else if (context.aiAgent.weapon.GetAmmo() == 0 && !context.aiAgent.weapon.GetIsCharging())
        {
            context.aiAgent.weapon.Recharge();
        }
        else
        {
            timer -= Time.deltaTime;
        }
        context.animator.SetFloat("Speed", context.agent.velocity.magnitude);
        Debug.Log("I am attacking you.");
        return State.Running;

    }
}

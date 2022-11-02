using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;
using Unity.VisualScripting;

public class AttackTarget : ActionNode
{
    public float timer = 1.0f;
    protected override void OnStart()
    {
        // Enable weapon inverse kinematics to allow for the AI to take aim.
        context.aiAgent.weaponIK.enabled = true;

        // Set what thee AI should aim at.
        context.aiAgent.weaponIK.targetTransform = blackboard.target.transform;

        // Pick a random stopping distance from the player.
        context.agent.stoppingDistance = Random.Range(context.aiAgent.aiData.minAttackStoppingDistance, context.aiAgent.aiData.maxAttackStoppingDistance);

        // Set the speed of the AI.
        context.agent.speed = context.aiAgent.aiData.walkspeed;

        // Move the AI towards the player.
        context.agent.updateRotation = true;
        context.agent.acceleration = context.aiAgent.aiData.acceleration;
        context.agent.destination = blackboard.moveToPosition;

        // Double checking that the animator is set to attack.
        context.animator.SetBool(context.aiAgent.aiData.attackParam, true);
    }

    protected override void OnStop()
    {
        context.aiAgent.weaponIK.weight = 0.0f;
        context.aiAgent.weaponIK.enabled = false;
        context.animator.SetBool(context.aiAgent.aiData.attackParam, false);
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

        // Every update, update the target and where to move.
        context.aiAgent.weaponIK.targetTransform = blackboard.target.transform;
        blackboard.moveToPosition = new Vector3(blackboard.target.transform.position.x, 0.0f, blackboard.target.transform.position.z);
        context.agent.destination = blackboard.moveToPosition;

        // Calculate the direction of the target between where the AI is aiming at, and the AI's position.
        Vector3 targetDirection = context.aiAgent.weaponIK.targetTransform.position - context.aiAgent.transform.position;

        // If the angle between the AI's forward direction and target direction is greater than the weaponIK angle limit, rotate the AI to look towards the target. 
        if (Mathf.Abs(Vector3.Angle(context.aiAgent.transform.forward, targetDirection)) >= context.aiAgent.weaponIK.angleLimit/2.0f)
        {
            context.aiAgent.transform.rotation = Quaternion.RotateTowards(context.aiAgent.transform.rotation, Quaternion.LookRotation(targetDirection), 45.0f * .2f);
        }

        
        // If you have ammo, you waited some time from your last shot, and the weapon is not charging, then shoot.
        if (context.aiAgent.weapon.GetAmmo() > 0 && timer <= 0 && !context.aiAgent.weapon.GetIsCharging())
        {
            // Shoot using AI_Shoot method, passing in the AI's inaccuracy from its data file.
            //context.aiAgent.weapon.AI_Shoot(context.aiAgent.aiData.xInaccuracy, context.aiAgent.aiData.yInaccuracy); ***
            context.aiAgent.weapon.photonView.RPC("AI_Shoot", Photon.Pun.RpcTarget.AllBuffered, context.aiAgent.aiData.xInaccuracy, context.aiAgent.aiData.yInaccuracy);

            // Pick a random time to wait to shoot next time.
            timer = Random.Range(context.aiAgent.aiData.minShootTimer, context.aiAgent.aiData.maxShootTimer);
        }
        else if (context.aiAgent.weapon.GetAmmo() == 0 && !context.aiAgent.weapon.GetIsCharging())
        {
            // If you are out of ammo, and the weapon isn't currentlyy charging, charge the weapon.
            context.aiAgent.weapon.photonView.RPC("Recharge", Photon.Pun.RpcTarget.AllBuffered);
        }
        else
        {
            // No action, decrement timer for next action.
            timer -= Time.deltaTime;
        }

        // Set the speed of the animator.
        context.animator.SetFloat(context.aiAgent.aiData.speedParam, context.agent.velocity.magnitude);
        return State.Running;

    }
}
